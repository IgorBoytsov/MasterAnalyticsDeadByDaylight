using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Killer;
using DBDAnalytics.CatalogService.Domain.ValueObjects.KillerPerkCategory;
using DBDAnalytics.CatalogService.Infrastructure.EF.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Configurations
{
    internal sealed class KillerPerkConfiguration : IEntityTypeConfiguration<KillerPerk>
    {
        public void Configure(EntityTypeBuilder<KillerPerk> builder)
        {
            builder.ToTable("KillerPerks");

            builder.HasKey(kp => kp.Id);

            /*__Ids__*/

            builder.Property(kp => kp.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever();

            builder.Property(kp => kp.OldId)
                .HasColumnName("OldId");

            builder.Property(kp => kp.KillerId)
                .HasColumnName("KillerId")
                .IsRequired();

            builder.Property(kp => kp.CategoryId)
                .HasColumnName("CategoryId")
                .HasConversion(
                    idCountry => idCountry.HasValue ? idCountry.Value.Value : (int?)null,
                    dbValue => dbValue.HasValue ? new KillerPerkCategoryId(dbValue.Value) : (KillerPerkCategoryId?)null)
                .IsRequired(false);

            /*__Название перка__*/

            builder.Property(kp => kp.Name)
                .HasColumnName("Name")
                .HasConversion(
                    name => name.Value,
                    dbValue => new KillerPerkName(dbValue))
                .HasMaxLength(KillerPerkName.MAX_LENGTH)
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .IsRequired();

            /*__Изображение__*/

            builder.Property(k => k.ImageKey)
                .HasColumnName("ImageKey")
                .HasConversion(
                    imageKey => imageKey != null ? imageKey.Value : null,
                    dbValue => dbValue != null ? ImageKey.Create(dbValue) : null);

            /*__Связи__*/

            builder.HasOne<KillerPerkCategory>()
                .WithMany()
                .HasForeignKey(kp => kp.CategoryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}