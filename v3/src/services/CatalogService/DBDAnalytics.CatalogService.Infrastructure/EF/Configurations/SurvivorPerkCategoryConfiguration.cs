using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.SurvivorPerkCategory;
using DBDAnalytics.CatalogService.Infrastructure.EF.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Configurations
{
    internal sealed class SurvivorPerkCategoryConfiguration : IEntityTypeConfiguration<SurvivorPerkCategory>
    {
        public void Configure(EntityTypeBuilder<SurvivorPerkCategory> builder)
        {
            builder.ToTable("SurvivorPerkCategories");

            builder.HasKey(spc => spc.Id);

            builder.Property(spc => spc.Id)
                .HasColumnName("Id")
                .HasConversion(
                    spc => spc.Value,
                    dbValue => new SurvivorPerkCategoryId(dbValue))
                .ValueGeneratedOnAdd();

            builder.Property(c => c.OldId)
                .HasColumnName("OldId");

            builder.Property(c => c.Name)
                .HasColumnName("Name")
                .HasConversion(
                    name => name.Value,
                    dbValue => new SurvivorPerkCategoryName(dbValue))
                .HasMaxLength(SurvivorPerkCategoryName.MAX_LENGTH)
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .IsRequired();
        }
    }
}