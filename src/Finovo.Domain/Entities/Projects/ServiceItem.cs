using Finovo.Domain.Common;
using Finovo.Domain.Enums;
using Finovo.Domain.ValueObjects;

namespace Finovo.Domain.Entities.Projects;

public class ServiceItem : BaseEntity
{
    public Guid ProjectId { get; private set; }
    public string Description { get; private set; }
    public decimal Hours { get; private set; }
    public Money HourlyRate { get; private set; }
    public TaxRate TaxRate { get; private set; }

    public Money TotalNet => HourlyRate.Multiply(Hours);
    public Money TaxAmount => TotalNet.Multiply((decimal)TaxRate / 100);
    public Money TotalGross => TotalNet.Multiply(1 + (decimal)TaxRate / 100);

    private ServiceItem() { Description = string.Empty; HourlyRate = Money.Zero(); } // EF Core

    public ServiceItem(Guid projectId, string description, decimal hours, Money hourlyRate, TaxRate taxRate = TaxRate.Standard)
    {
        if (projectId == Guid.Empty) throw new ArgumentException("ProjectId is required.", nameof(projectId));
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        if (hours <= 0) throw new ArgumentException("Hours must be greater than zero.", nameof(hours));

        ProjectId = projectId;
        Description = description.Trim();
        Hours = hours;
        HourlyRate = hourlyRate;
        TaxRate = taxRate;
    }

    internal void Update(string description, decimal hours, Money hourlyRate, TaxRate taxRate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        if (hours <= 0) throw new ArgumentException("Hours must be greater than zero.", nameof(hours));

        Description = description.Trim();
        Hours = hours;
        HourlyRate = hourlyRate;
        TaxRate = taxRate;
        SetUpdated();
    }
}
