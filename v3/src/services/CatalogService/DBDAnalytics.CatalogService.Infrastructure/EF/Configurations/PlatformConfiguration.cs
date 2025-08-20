using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Platform;
using DBDAnalytics.CatalogService.Infrastructure.EF.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Configurations
{
    internal sealed class PlatformConfiguration : IEntityTypeConfiguration<Platform>
    {
        public void Configure(EntityTypeBuilder<Platform> builder)
        {
            builder.ToTable("Platforms");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("Id")
                .HasConversion(
                    platformId => platformId.Value,
                    dbValue => new PlatformId(dbValue))
                .ValueGeneratedOnAdd();

            builder.Property(p => p.OldId)
               .HasColumnName("OldId");

            builder.Property(p => p.Name)
                .HasColumnName("Name")
                .HasConversion(
                    name => name.Value,
                    dbValue => new PlatformName(dbValue))
                .HasMaxLength(PlatformName.MAX_LENGTH)
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .IsRequired();
        }
    }
}