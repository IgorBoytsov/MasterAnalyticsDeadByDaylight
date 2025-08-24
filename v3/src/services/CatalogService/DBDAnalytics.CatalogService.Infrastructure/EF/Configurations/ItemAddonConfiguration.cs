using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.ItemAddon;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Rarity;
using DBDAnalytics.CatalogService.Infrastructure.EF.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Configurations
{
    internal sealed class ItemAddonConfiguration : IEntityTypeConfiguration<ItemAddon>
    {
        public void Configure(EntityTypeBuilder<ItemAddon> builder)
        {
            builder.ToTable("ItemAddons");

            builder.HasKey(x => x.Id);

            builder.Property(ia => ia.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever();

            builder.Property(ia => ia.OldId)
                .HasColumnName("OldId");

            builder.Property(ia => ia.ItemId)
                .HasColumnName("ItemId")
                .IsRequired();

            builder.Property(ia => ia.RarityId)
                .HasColumnName("RarityId")
                .HasConversion(
                    rarityId => rarityId != null ? rarityId.Value : (int?)null,
                    dbValue => dbValue != null ? new RarityId(dbValue.Value) : (RarityId?)null);

            builder.Property(ia => ia.Name)
                .HasColumnName("Name")
                .HasConversion(
                    name => name.Value,
                    dbValue => new ItemAddonName(dbValue))
                .HasMaxLength(ItemAddonName.MAX_LENGTH)
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .IsRequired();

            builder.Property(ia => ia.ImageKey)
                .HasColumnName("ImageKey")
                .HasConversion(
                    image => image != null ? image.Value : null,
                    dbValue => dbValue != null ? ImageKey.Create(dbValue) : null)
                .IsRequired(false);
        }
    }
}