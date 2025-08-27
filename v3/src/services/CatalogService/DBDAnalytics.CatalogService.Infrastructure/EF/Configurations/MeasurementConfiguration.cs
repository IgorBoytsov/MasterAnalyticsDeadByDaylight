using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Measurement;
using DBDAnalytics.CatalogService.Infrastructure.EF.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Configurations
{
    internal sealed class MeasurementConfiguration : IEntityTypeConfiguration<Measurement>
    {
        public void Configure(EntityTypeBuilder<Measurement> builder)
        {
            builder.ToTable("Measurements");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever();

            builder.Property(m => m.OldId)
             .HasColumnName("OldId");

            /*Название*/

            builder.Property(m => m.Name)
                .HasColumnName("Name")
                .HasConversion(
                    name => name.Value,
                    dbValue => new MeasurementName(dbValue))
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .IsRequired();

            /*__Связи__*/

            builder.HasMany(m => m.Maps)
                .WithOne(m => m.Measurement)
                .HasForeignKey(m => m.MeasurementId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(m => m.Maps).HasField("_maps").UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}