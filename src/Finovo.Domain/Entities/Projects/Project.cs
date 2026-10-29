using Finovo.Domain.Common;
using Finovo.Domain.Enums;
using Finovo.Domain.Events;
using Finovo.Domain.Exceptions;
using Finovo.Domain.ValueObjects;

namespace Finovo.Domain.Entities.Projects;

public class Project : AggregateRoot
{
    public Guid TenantId { get; private set; }
    public Guid CustomerId { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public ProjectStatus Status { get; private set; }

    private readonly List<ServiceItem> _serviceItems = [];
    public IReadOnlyList<ServiceItem> ServiceItems => _serviceItems.AsReadOnly();

    public Money TotalNet => _serviceItems.Aggregate(Money.Zero(), (acc, item) => acc.Add(item.TotalNet));
    public Money TotalGross => _serviceItems.Aggregate(Money.Zero(), (acc, item) => acc.Add(item.TotalGross));

    private Project() { Name = string.Empty; } // EF Core

    public Project(Guid tenantId, Guid customerId, string name, string? description = null)
    {
        if (tenantId == Guid.Empty) throw new ArgumentException("TenantId is required.", nameof(tenantId));
        if (customerId == Guid.Empty) throw new ArgumentException("CustomerId is required.", nameof(customerId));
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        TenantId = tenantId;
        CustomerId = customerId;
        Name = name.Trim();
        Description = description?.Trim();
        Status = ProjectStatus.Open;
    }

    public void Update(string name, string? description)
    {
        if (Status is ProjectStatus.Archived)
            throw new DomainException("Cannot update an archived project.");

        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name.Trim();
        Description = description?.Trim();
        SetUpdated();
    }

    public ServiceItem AddServiceItem(string description, decimal hours, Money hourlyRate, TaxRate taxRate = TaxRate.Standard)
    {
        if (Status is ProjectStatus.Invoiced or ProjectStatus.Archived)
            throw new DomainException("Cannot add service items to an invoiced or archived project.");

        var item = new ServiceItem(Id, description, hours, hourlyRate, taxRate);
        _serviceItems.Add(item);
        SetUpdated();
        return item;
    }

    public void UpdateServiceItem(Guid serviceItemId, string description, decimal hours, Money hourlyRate, TaxRate taxRate)
    {
        if (Status is ProjectStatus.Invoiced or ProjectStatus.Archived)
            throw new DomainException("Cannot update service items of an invoiced or archived project.");

        var item = _serviceItems.FirstOrDefault(x => x.Id == serviceItemId)
            ?? throw new DomainException("Service item not found.");

        item.Update(description, hours, hourlyRate, taxRate);
        SetUpdated();
    }

    public void RemoveServiceItem(Guid serviceItemId)
    {
        if (Status is ProjectStatus.Invoiced or ProjectStatus.Archived)
            throw new DomainException("Cannot remove service items from an invoiced or archived project.");

        var item = _serviceItems.FirstOrDefault(x => x.Id == serviceItemId)
            ?? throw new DomainException("Service item not found.");

        _serviceItems.Remove(item);
        SetUpdated();
    }

    public void Complete()
    {
        if (Status != ProjectStatus.Open)
            throw new DomainException("Only open projects can be completed.");
        if (_serviceItems.Count == 0)
            throw new DomainException("Cannot complete a project without service items.");

        Status = ProjectStatus.Completed;
        SetUpdated();
        AddDomainEvent(new ProjectCompletedDomainEvent(Id, TenantId));
    }

    public void Reopen()
    {
        if (Status != ProjectStatus.Completed)
            throw new DomainException("Only completed projects can be reopened.");

        Status = ProjectStatus.Open;
        SetUpdated();
    }

    public void MarkAsInvoiced()
    {
        if (Status != ProjectStatus.Completed)
            throw new DomainException("Only completed projects can be marked as invoiced.");

        Status = ProjectStatus.Invoiced;
        SetUpdated();
    }

    public void Archive()
    {
        if (Status == ProjectStatus.Archived)
            throw new DomainException("Project is already archived.");

        Status = ProjectStatus.Archived;
        SetUpdated();
    }
}
