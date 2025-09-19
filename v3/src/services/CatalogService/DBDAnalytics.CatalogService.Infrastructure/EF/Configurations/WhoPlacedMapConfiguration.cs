using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.WhoPlacedMap;
using DBDAnalytics.CatalogService.Infrastructure.EF.Constants;
using DBDAnalytics.Shared.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Configurations
{
    internal sealed class WhoPlacedMapConfiguration : IEntityTypeConfiguration<WhoPlacedMap>
    {
        public void Configure(EntityTypeBuilder<WhoPlacedMap> builder)
        {
            builder.ToTable("WhoPlacedMaps");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.Id)
                .HasColumnName("Id")
                .HasConversion(
                    place => place.Value,
                    dbValue => new WhoPlacedMapId(dbValue))
                .ValueGeneratedOnAdd();

            builder.Property(m => m.Name)
                .HasColumnName("Name")
                .HasConversion(
                    name => name.Value,
                    dbValue => new PlacedMapName(dbValue))
                .HasMaxLength(PlacedMapName.MAX_LENGTH)
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .IsRequired();
        }
    }
}