using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.OfferingCategory;
using DBDAnalytics.CatalogService.Infrastructure.EF.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Configurations
{
    internal sealed class OfferingCategoryConfiguration : IEntityTypeConfiguration<OfferingCategory>
    {
        public void Configure(EntityTypeBuilder<OfferingCategory> builder)
        {
            builder.ToTable("OfferingCategories");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("Id")
                .HasConversion(
                    catId => catId.Value,
                    dbValue => new OfferingCategoryId(dbValue))
                .ValueGeneratedOnAdd();

            builder.Property(c => c.OldId)
                .HasColumnName("OldId");

            builder.Property(c => c.Name)
                .HasColumnName("Name")
                .HasConversion(
                    name => name.Value,
                    dbValue => new OfferingCategoryName(dbValue))
                .HasMaxLength(OfferingCategoryName.MAX_LENGTH)
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .IsRequired();
        }
    }
}