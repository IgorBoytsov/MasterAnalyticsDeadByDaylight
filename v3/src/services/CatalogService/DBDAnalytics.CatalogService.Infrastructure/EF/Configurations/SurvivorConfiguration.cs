using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Survivor;
using DBDAnalytics.CatalogService.Infrastructure.EF.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Configurations
{
    internal sealed class SurvivorConfiguration : IEntityTypeConfiguration<Survivor>
    {
        public void Configure(EntityTypeBuilder<Survivor> builder)
        {
            builder.ToTable("Survivors");

            builder.HasKey(s => s.Id);

            /*__Ids__*/

            builder.Property(s => s.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever();

            builder.Property(s => s.OldId)
                .HasColumnName("OldId");

            /*Название*/

            builder.Property(s => s.Name)
                .HasColumnName("Name")
                .HasConversion(
                    name => name.Value,
                    dbValue => new SurvivorName(dbValue))
                .HasMaxLength(SurvivorName.MAX_LENGTH)
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .IsRequired();

            /*__Изображение__*/

            builder.Property(s => s.ImageKey)
                .HasConversion(
                    imageKey => imageKey != null ? imageKey.Value : null,
                    dbValue => dbValue != null ? ImageKey.Create(dbValue) : null);

            /*__Связи__*/

            builder.HasMany(s => s.SurvivorPerks)
                .WithOne(sp => sp.Survivor)
                .HasForeignKey(sp => sp.SurvivorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(s => s.SurvivorPerks).HasField("_survivorPerks").UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}