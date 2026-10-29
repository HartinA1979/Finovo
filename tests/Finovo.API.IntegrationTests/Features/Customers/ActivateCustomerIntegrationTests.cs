using System.Net;
using Finovo.API.IntegrationTests.Common;
using Finovo.Application.Common.Interfaces;
using Finovo.Domain.Entities.Customers;
using Finovo.Domain.Entities.Tenants;
using Finovo.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Finovo.API.IntegrationTests.Features.Customers;

public class ActivateCustomerIntegrationTests : IClassFixture<FinovoWebApplicationFactory>
{
    private const string TenantSlug = "activate-test";

    private readonly FinovoWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ActivateCustomerIntegrationTests(FinovoWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Add("X-Tenant-Slug", TenantSlug);
    }

    [Fact]
    public async Task Activate_DeactivatedCustomer_ReturnsNoContentAndActivatesCustomer()
    {
        var tenantId = await EnsureTenantAsync();
        var customerId = await SeedDeactivatedCustomerAsync(tenantId);

        var response = await _client.PostAsync($"/api/customers/{customerId}/activate", null);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var customer = await db.Customers
            .IgnoreQueryFilters()
            .SingleAsync(c => c.Id == customerId);

        customer.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task Activate_UnknownCustomer_ReturnsNotFound()
    {
        await EnsureTenantAsync();

        var response = await _client.PostAsync($"/api/customers/{Guid.NewGuid()}/activate", null);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task<Guid> EnsureTenantAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var tenant = await db.Tenants.FirstOrDefaultAsync(t => t.Subdomain == TenantSlug);
        if (tenant is not null)
            return tenant.Id;

        tenant = new Tenant("Test Tenant", TenantSlug);
        db.Tenants.Add(tenant);
        await db.SaveChangesAsync();

        return tenant.Id;
    }

    private async Task<Guid> SeedDeactivatedCustomerAsync(Guid tenantId)
    {
        using var scope = _factory.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<ITenantContextSetter>().SetTenantId(tenantId);
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var customer = new Customer(tenantId, "Test Customer");
        customer.Deactivate();
        db.Customers.Add(customer);
        await db.SaveChangesAsync();

        return customer.Id;
    }
}
