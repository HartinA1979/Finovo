using Finovo.Domain.Entities.Projects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finovo.Infrastructure.Persistence.Configurations;

internal sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TenantId).IsRequired();
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => x.CustomerId);

        builder.Property(x => x.Name).HasMaxLength(300).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.Status).IsRequired();

        builder.HasMany(x => x.ServiceItems)
            .WithOne()
            .HasForeignKey(si => si.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(x => x.TotalNet);
        builder.Ignore(x => x.TotalGross);

    }
}
