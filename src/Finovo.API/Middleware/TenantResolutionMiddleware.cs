using Finovo.API.Settings;
using Finovo.Application.Common.Interfaces;
using Finovo.Domain.Enums;
using Finovo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Finovo.API.Middleware;

public sealed class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _settings;
    private readonly IMemoryCache _cache;
    private readonly ILogger<TenantResolutionMiddleware> _logger;

    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    // Paths that bypass tenant resolution (health checks, OpenAPI, etc.)
    private static readonly string[] BypassPrefixes =
    [
        "/health",
        "/swagger",
        "/openapi",
        "/favicon.ico"
    ];

    public TenantResolutionMiddleware(
        RequestDelegate next,
        IOptions<AppSettings> settings,
        IMemoryCache cache,
        ILogger<TenantResolutionMiddleware> logger)
    {
        _next = next;
        _settings = settings.Value;
        _cache = cache;
        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ITenantContextSetter tenantContextSetter,
        AppDbContext dbContext)
    {
        var path = context.Request.Path.Value ?? string.Empty;

        if (ShouldBypass(path))
        {
            await _next(context);
            return;
        }

        var subdomain = ExtractSubdomain(context.Request.Host.Host);

        // Development fallback: read tenant slug from custom header
        if (string.IsNullOrEmpty(subdomain) && _settings.DevTenantHeaderFallback)
        {
            subdomain = context.Request.Headers["X-Tenant-Slug"].FirstOrDefault();
        }

        if (string.IsNullOrEmpty(subdomain))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Tenant could not be determined from the request.");
            return;
        }

        // Admin subdomain — no tenant context, just flag as admin
        if (subdomain.Equals(_settings.AdminSubdomain, StringComparison.OrdinalIgnoreCase))
        {
            tenantContextSetter.SetAdminContext();
            await _next(context);
            return;
        }

        var entry = await ResolveTenantAsync(subdomain, dbContext);

        if (entry is null)
        {
            _logger.LogWarning("Tenant not found for subdomain '{Subdomain}'", subdomain);
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync("Tenant not found.");
            return;
        }

        if (!entry.IsActive)
        {
            _logger.LogWarning("Tenant '{Subdomain}' has an inactive license", subdomain);
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Tenant license is not active.");
            return;
        }

        tenantContextSetter.SetTenantId(entry.TenantId);
        await _next(context);
    }

    private async Task<TenantCacheEntry?> ResolveTenantAsync(string subdomain, AppDbContext dbContext)
    {
        var cacheKey = $"tenant_slug:{subdomain}";

        if (_cache.TryGetValue(cacheKey, out TenantCacheEntry? cached))
            return cached;

        var tenant = await dbContext.Tenants
            .Where(t => t.Subdomain == subdomain)
            .Select(t => new
            {
                t.Id,
                t.LicenseStatus,
                t.LicenseExpiresAt
            })
            .FirstOrDefaultAsync();

        if (tenant is null)
            return null;

        var isActive = tenant.LicenseStatus is LicenseStatus.Trial or LicenseStatus.Active
            && (tenant.LicenseExpiresAt == null || tenant.LicenseExpiresAt > DateTime.UtcNow);

        var entry = new TenantCacheEntry(tenant.Id, isActive);
        _cache.Set(cacheKey, entry, CacheDuration);

        return entry;
    }

    private string? ExtractSubdomain(string host)
    {
        // Strip port if present (e.g. "hammer.finovo.at:443" → "hammer.finovo.at")
        var hostOnly = host.Contains(':') ? host[..host.LastIndexOf(':')] : host;

        var suffix = "." + _settings.BaseDomain;
        if (hostOnly.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
        {
            var sub = hostOnly[..^suffix.Length];
            return string.IsNullOrEmpty(sub) ? null : sub;
        }

        return null;
    }

    private static bool ShouldBypass(string path) =>
        BypassPrefixes.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase));

    private sealed record TenantCacheEntry(Guid TenantId, bool IsActive);
}
