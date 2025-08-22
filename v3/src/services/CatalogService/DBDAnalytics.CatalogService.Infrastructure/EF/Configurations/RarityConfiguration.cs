using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Rarity;
using DBDAnalytics.CatalogService.Infrastructure.EF.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Configurations
{
    internal sealed class RarityConfiguration : IEntityTypeConfiguration<Rarity>
    {
        public void Configure(EntityTypeBuilder<Rarity> builder)
        {
            builder.ToTable("Rarities");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasColumnName("Id")
                .HasConversion(
                    rarityId => rarityId.Value,
                    dbValue => new RarityId(dbValue))
                .ValueGeneratedOnAdd();

            builder.Property(r => r.OldId)
               .HasColumnName("OldId");

            builder.Property(r => r.Name)
                .HasColumnName("Name")
                .HasConversion(
                    name => name.Value,
                    dbValue => new RarityName(dbValue))
                .HasMaxLength(RarityName.MAX_LENGTH)
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .IsRequired();

            builder.HasMany(r => r.Offerings)
                .WithOne(o => o.Rarity)
                .HasForeignKey(o => o.RarityId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Navigation(r => r.Offerings).HasField("_offerings").UsePropertyAccessMode(PropertyAccessMode.Field);

        }
    }
}