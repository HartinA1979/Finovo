using Finovo.Domain.Common;
using Finovo.Domain.Enums;
using Finovo.Domain.Exceptions;
using Finovo.Domain.ValueObjects;

namespace Finovo.Domain.Entities.Financials;

public class Income : AggregateRoot
{
    public Guid TenantId { get; private set; }
    public string Description { get; private set; }
    public Money Amount { get; private set; }
    public DateTime Date { get; private set; }
    public TaxRate TaxRate { get; private set; }
    public string? Category { get; private set; }

    /// <summary>
    /// Set when income is automatically created from a paid invoice.
    /// </summary>
    public Guid? InvoiceId { get; private set; }

    private Income() { Description = string.Empty; Amount = Money.Zero(); } // EF Core

    public Income(
        Guid tenantId,
        string description,
        Money amount,
        DateTime date,
        TaxRate taxRate,
        string? category = null,
        Guid? invoiceId = null)
    {
        if (tenantId == Guid.Empty) throw new ArgumentException("TenantId is required.", nameof(tenantId));
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        TenantId = tenantId;
        Description = description.Trim();
        Amount = amount;
        Date = date.Date;
        TaxRate = taxRate;
        Category = category?.Trim();
        InvoiceId = invoiceId;
    }

    public void Update(string description, Money amount, DateTime date, TaxRate taxRate, string? category)
    {
        if (InvoiceId.HasValue)
            throw new DomainException("Income linked to an invoice cannot be manually updated.");

        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        Description = description.Trim();
        Amount = amount;
        Date = date.Date;
        TaxRate = taxRate;
        Category = category?.Trim();
        SetUpdated();
    }
}
