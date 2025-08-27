using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Item;
using DBDAnalytics.CatalogService.Infrastructure.EF.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Configurations
{
    internal sealed class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Item");

            builder.HasKey(x => x.Id);

            builder.Property(i => i.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever();

            builder.Property(i => i.OldId)
                .HasColumnName("OldId");

            builder.Property(i => i.Name)
                .HasColumnName("Name")
                .HasConversion(
                    name => name.Value,
                    dbValue => new ItemName(dbValue))
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .HasMaxLength(ItemName.MAX_LENGTH);

            builder.Property(i => i.ImageKey)
                .HasColumnName("ImageKey")
                .HasConversion(
                    image => image != null ? image.Value : null,
                    dbValue => dbValue != null ? ImageKey.Create(dbValue) : null)
                .IsRequired(false);

            builder.HasMany(i => i.ItemAddons)
                .WithOne(ia => ia.Item)
                .HasForeignKey(i => i.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(i => i.ItemAddons).HasField("_itemAddons").UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}