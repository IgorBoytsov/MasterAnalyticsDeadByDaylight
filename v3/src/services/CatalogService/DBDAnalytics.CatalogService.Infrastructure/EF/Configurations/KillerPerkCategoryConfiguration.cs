using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Killer;
using DBDAnalytics.CatalogService.Domain.ValueObjects.KillerPerkCategory;
using DBDAnalytics.CatalogService.Infrastructure.EF.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Configurations
{
    internal sealed class KillerPerkCategoryConfiguration : IEntityTypeConfiguration<KillerPerkCategory>
    {
        public void Configure(EntityTypeBuilder<KillerPerkCategory> builder)
        {
            builder.ToTable("KillerPerkCategories");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("Id")
                .HasConversion(
                    catId => catId.Value,
                    dbValue => new KillerPerkCategoryId(dbValue))
                .ValueGeneratedOnAdd();

            builder.Property(c => c.OldId)
                .HasColumnName("OldId");

            builder.Property(c => c.Name)
                .HasColumnName("Name")
                .HasConversion(
                    name => name.Value,
                    dbValue => new KillerPerkCategoryName(dbValue))
                .HasMaxLength(KillerPerkCategoryName.MAX_LENGTH)
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .IsRequired();
        }
    }
}