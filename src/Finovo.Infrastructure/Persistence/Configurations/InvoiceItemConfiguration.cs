using Finovo.Domain.Entities.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finovo.Infrastructure.Persistence.Configurations;

internal sealed class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
{
    public void Configure(EntityTypeBuilder<InvoiceItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.InvoiceId).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Hours).HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.TaxRate).IsRequired();

        builder.OwnsOne(x => x.HourlyRate, m =>
        {
            m.Property(x => x.Amount)
                .HasColumnName("HourlyRate_Amount")
                .HasPrecision(18, 4)
                .IsRequired();
            m.Property(x => x.Currency)
                .HasColumnName("HourlyRate_Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Ignore(x => x.TotalNet);
        builder.Ignore(x => x.TaxAmount);
        builder.Ignore(x => x.TotalGross);
    }
}
