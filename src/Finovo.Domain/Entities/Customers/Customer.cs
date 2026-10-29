using Finovo.Domain.Common;
using Finovo.Domain.ValueObjects;

namespace Finovo.Domain.Entities.Customers;

public class Customer : AggregateRoot
{
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public string? CompanyName { get; private set; }
    public Email? Email { get; private set; }
    public string? Phone { get; private set; }
    public Address? Address { get; private set; }
    public string? VatNumber { get; private set; }
    public string? Notes { get; private set; }
    public bool IsActive { get; private set; } = true;

    private Customer() { Name = string.Empty; } // EF Core

    public Customer(
        Guid tenantId,
        string name,
        string? companyName = null,
        Email? email = null,
        string? phone = null,
        Address? address = null,
        string? vatNumber = null)
    {
        if (tenantId == Guid.Empty) throw new ArgumentException("TenantId is required.", nameof(tenantId));
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        TenantId = tenantId;
        Name = name.Trim();
        CompanyName = companyName?.Trim();
        Email = email;
        Phone = phone?.Trim();
        Address = address;
        VatNumber = vatNumber?.Trim();
    }

    public void Update(
        string name,
        string? companyName,
        Email? email,
        string? phone,
        Address? address,
        string? vatNumber,
        string? notes)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name.Trim();
        CompanyName = companyName?.Trim();
        Email = email;
        Phone = phone?.Trim();
        Address = address;
        VatNumber = vatNumber?.Trim();
        Notes = notes?.Trim();
        SetUpdated();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdated();
    }

    public void Activate()
    {
        IsActive = true;
        SetUpdated();
    }
}
