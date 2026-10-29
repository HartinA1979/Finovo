using Finovo.Domain.Common;
using Finovo.Domain.Enums;
using Finovo.Domain.ValueObjects;

namespace Finovo.Domain.Entities.Financials;

public class Expense : AggregateRoot
{
    public Guid TenantId { get; private set; }
    public string Description { get; private set; }
    public Money Amount { get; private set; }
    public DateTime Date { get; private set; }
    public TaxRate TaxRate { get; private set; }
    public string? Category { get; private set; }

    private Expense() { Description = string.Empty; Amount = Money.Zero(); } // EF Core

    public Expense(
        Guid tenantId,
        string description,
        Money amount,
        DateTime date,
        TaxRate taxRate,
        string? category = null)
    {
        if (tenantId == Guid.Empty) throw new ArgumentException("TenantId is required.", nameof(tenantId));
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        TenantId = tenantId;
        Description = description.Trim();
        Amount = amount;
        Date = date.Date;
        TaxRate = taxRate;
        Category = category?.Trim();
    }

    public void Update(string description, Money amount, DateTime date, TaxRate taxRate, string? category)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        Description = description.Trim();
        Amount = amount;
        Date = date.Date;
        TaxRate = taxRate;
        Category = category?.Trim();
        SetUpdated();
    }
}
