namespace Finovo.API.Settings;

public sealed class AppSettings
{
    public const string SectionName = "App";

    public string BaseDomain { get; set; } = "finovo.at";
    public string AdminSubdomain { get; set; } = "admin";

    /// <summary>
    /// Enables X-Tenant-Slug header as subdomain fallback. Development only — never enable in production.
    /// </summary>
    public bool DevTenantHeaderFallback { get; set; } = false;
}
