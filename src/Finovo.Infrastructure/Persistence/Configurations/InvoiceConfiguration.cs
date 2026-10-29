using Finovo.Domain.Entities.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finovo.Infrastructure.Persistence.Configurations;

internal sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.ProjectId).IsRequired();
        builder.Property(x => x.CustomerId).IsRequired();
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => x.ProjectId);

        builder.Property(x => x.IssueDate).HasColumnType("date").IsRequired();
        builder.Property(x => x.DueDate).HasColumnType("date").IsRequired();
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.Notes).HasMaxLength(2000);

        builder.OwnsOne(x => x.InvoiceNumber, n =>
        {
            n.Property(x => x.Year)
                .HasColumnName("InvoiceNumber_Year")
                .IsRequired();
            n.Property(x => x.SequenceNumber)
                .HasColumnName("InvoiceNumber_Sequence")
                .IsRequired();

            // Unique invoice number per tenant per year enforced via unique index
            n.HasIndex(x => new { x.Year, x.SequenceNumber })
                .HasDatabaseName("IX_Invoices_TenantId_InvoiceNumber")
                .IsUnique();
        });

        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(ii => ii.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(x => x.TotalNet);
        builder.Ignore(x => x.TotalGross);
        builder.Ignore(x => x.TaxAmount);

    }
}
