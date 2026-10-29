using Finovo.Domain.Entities.Financials;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finovo.Infrastructure.Persistence.Configurations;

internal sealed class IncomeConfiguration : IEntityTypeConfiguration<Income>
{
    public void Configure(EntityTypeBuilder<Income> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TenantId).IsRequired();
        builder.HasIndex(x => x.TenantId);

        builder.Property(x => x.Description).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Date).HasColumnType("date").IsRequired();
        builder.Property(x => x.TaxRate).IsRequired();
        builder.Property(x => x.Category).HasMaxLength(100);
        builder.Property(x => x.InvoiceId);

        builder.OwnsOne(x => x.Amount, m =>
        {
            m.Property(x => x.Amount)
                .HasColumnName("Amount")
                .HasPrecision(18, 4)
                .IsRequired();
            m.Property(x => x.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

    }
}
