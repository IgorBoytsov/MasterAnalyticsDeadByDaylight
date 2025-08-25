using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Offering;
using DBDAnalytics.CatalogService.Domain.ValueObjects.OfferingCategory;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Rarity;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Role;
using DBDAnalytics.CatalogService.Infrastructure.EF.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Configurations
{
    internal sealed class OfferingConfiguration : IEntityTypeConfiguration<Offering>
    {
        public void Configure(EntityTypeBuilder<Offering> builder)
        {
            builder.ToTable("Offerings");

            builder.HasKey(o => o.Id);

            /*__Ids__*/

            builder.Property(o => o.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever();

            builder.Property(o => o.OldId)
                .HasColumnName("OldId");

            builder.Property(o => o.RoleId)
                .HasColumnName("RoleId")
                .HasConversion(
                    roleId => roleId.Value,
                    dbValue => new RoleId(dbValue))
                .IsRequired();

            builder.Property(o => o.RarityId)
                .HasColumnName("RarityId")
                .HasConversion(
                    rarityId => rarityId.HasValue ? rarityId.Value.Value : (int?)null,
                    dbValue => dbValue.HasValue ? new RarityId(dbValue.Value) : (RarityId?)null)
                .IsRequired(false);

            builder.Property(o => o.CategoryId)
                .HasColumnName("OfferingCategoryId")
                .HasConversion(
                    catId => catId.HasValue ? catId.Value.Value : (int?)null,
                    dbValue => dbValue.HasValue ? new OfferingCategoryId(dbValue.Value) : (OfferingCategoryId?)null)
                .IsRequired(false);

            /*__Название подношения__*/

            builder.Property(o => o.Name)
                .HasColumnName("Name")
                .HasConversion(
                    name => name.Value,
                    dbValue => new OfferingName(dbValue))
                .HasMaxLength(OfferingName.MAX_LENGTH)
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .IsRequired(); ;

            /*__Изображение__*/

            builder.Property(o => o.ImageKey)
                .HasColumnName("ImageKey")
                .HasConversion(
                    imageKey => imageKey != null ? imageKey.Value : null,
                    dbValue => dbValue != null ? ImageKey.Create(dbValue) : null)
                .IsRequired(false);

            /*__Связи__*/

            builder.HasOne<Role>()
                .WithMany()
                .HasForeignKey(o => o.RoleId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<OfferingCategory>()
                .WithMany()
                .HasForeignKey(o => o.CategoryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne<Rarity>()
                .WithMany()
                .HasForeignKey(o => o.RarityId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}