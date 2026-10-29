using Finovo.Domain.Entities.Customers;
using Finovo.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finovo.Infrastructure.Persistence.Configurations;

internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TenantId).IsRequired();
        builder.HasIndex(x => x.TenantId);

        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.CompanyName).HasMaxLength(200);
        builder.Property(x => x.Phone).HasMaxLength(50);
        builder.Property(x => x.VatNumber).HasMaxLength(50);
        builder.Property(x => x.Notes).HasMaxLength(2000);

        builder.Property(x => x.Email)
            .HasConversion(
                e => e != null ? e.Value : null,
                v => v != null ? new Email(v) : null)
            .HasMaxLength(256)
            .HasColumnName("Email");

        builder.OwnsOne(x => x.Address, a =>
        {
            a.Property(x => x.Street).HasColumnName("Address_Street").HasMaxLength(200);
            a.Property(x => x.City).HasColumnName("Address_City").HasMaxLength(100);
            a.Property(x => x.ZipCode).HasColumnName("Address_ZipCode").HasMaxLength(20);
            a.Property(x => x.Country).HasColumnName("Address_Country").HasMaxLength(100);
        });

    }
}
