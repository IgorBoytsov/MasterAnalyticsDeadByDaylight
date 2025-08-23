using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.SurvivorPerk;
using DBDAnalytics.CatalogService.Domain.ValueObjects.SurvivorPerkCategory;
using DBDAnalytics.CatalogService.Infrastructure.EF.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Configurations
{
    internal class SurvivorPerkConfiguration : IEntityTypeConfiguration<SurvivorPerk>
    {
        public void Configure(EntityTypeBuilder<SurvivorPerk> builder)
        {
            builder.ToTable("SurvivorPerks");

            builder.HasKey(sp => sp.Id);

            /*__Ids__*/

            builder.Property(sp => sp.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever();

            builder.Property(s => s.OldId)
                .HasColumnName("OldId");

            builder.Property(sp => sp.SurvivorId)
                .HasColumnName("SurvivorId")
                .IsRequired();

            builder.Property(sp => sp.CategoryId)
                .HasColumnName("CategoryId")
                .HasConversion(
                    categoryId => categoryId.HasValue ? categoryId.Value.Value : (int?)null,
                    dbValue => dbValue.HasValue ? new SurvivorPerkCategoryId(dbValue.Value) : null);

            /*Название*/

            builder.Property(sp => sp.Name)
                .HasColumnName("Name")
                .HasConversion(
                    name => name.Value,
                    dbValue => new SurvivorPerkName(dbValue))
                .UseCollation(PostgresConstants.COLLATION_NAME);

            /*__Изображение__*/

            builder.Property(s => s.ImageKey)
                .HasConversion(
                    imageKey => imageKey != null ? imageKey.Value : null,
                    dbValue => dbValue != null ? ImageKey.Create(dbValue) : null);

            /*__Связи__*/

            builder.HasOne<SurvivorPerkCategory>()
               .WithMany()
               .HasForeignKey(sp => sp.CategoryId)
               .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
