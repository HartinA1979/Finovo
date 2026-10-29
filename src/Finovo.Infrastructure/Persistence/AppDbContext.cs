using Finovo.Application.Common.Interfaces;
using Finovo.Application.Common.Models;
using Finovo.Domain.Common;
using Finovo.Domain.Entities.Customers;
using Finovo.Domain.Entities.Financials;
using Finovo.Domain.Entities.Invoices;
using Finovo.Domain.Entities.Projects;
using Finovo.Domain.Entities.Tenants;
using Finovo.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Finovo.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly ITenantContext _tenantContext;
    private readonly IPublisher _publisher;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        ITenantContext tenantContext,
        IPublisher publisher)
        : base(options)
    {
        _tenantContext = tenantContext;
        _publisher = publisher;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ServiceItem> ServiceItems => Set<ServiceItem>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();
    public DbSet<Income> Incomes => Set<Income>();
    public DbSet<Expense> Expenses => Set<Expense>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Global Query Filters — applied here so the lambda captures _tenantContext (scoped per request)
        builder.Entity<Customer>().HasQueryFilter(x => x.TenantId == _tenantContext.CurrentTenantId);
        builder.Entity<Project>().HasQueryFilter(x => x.TenantId == _tenantContext.CurrentTenantId);
        builder.Entity<Invoice>().HasQueryFilter(x => x.TenantId == _tenantContext.CurrentTenantId);
        builder.Entity<Income>().HasQueryFilter(x => x.TenantId == _tenantContext.CurrentTenantId);
        builder.Entity<Expense>().HasQueryFilter(x => x.TenantId == _tenantContext.CurrentTenantId);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var aggregatesWithEvents = ChangeTracker.Entries<AggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Count > 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = aggregatesWithEvents
            .SelectMany(a => a.DomainEvents)
            .ToList();

        aggregatesWithEvents.ForEach(a => a.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
            await _publisher.Publish(new DomainEventNotification(domainEvent), cancellationToken);

        return result;
    }
}
