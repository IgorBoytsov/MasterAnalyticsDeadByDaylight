using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.KillerAddon;
using DBDAnalytics.CatalogService.Infrastructure.EF.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Configurations
{
    internal sealed class KillerAddonConfiguration : IEntityTypeConfiguration<KillerAddon>
    {
        public void Configure(EntityTypeBuilder<KillerAddon> builder)
        {
            builder.ToTable("KillerAddons");

            builder.HasKey(ka => ka.Id);

            /*__Ids__*/
            builder.Property(ka => ka.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever();

            builder.Property(ka => ka.KillerId)
                .HasColumnName("KillerId")
                .IsRequired();

            builder.Property(ka => ka.Name)
                .HasColumnName("Name")
                .HasConversion(
                    name => name.Value,
                    dbValue => new KillerAddonName(dbValue))
                .HasMaxLength(KillerAddonName.MAX_LENGTH)
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .IsRequired();

            builder.Property(ka => ka.ImageKey)
                .HasColumnName("ImageKey")
                .HasConversion(
                    imageKey => imageKey != null ? imageKey.Value : null,
                    dbValue => dbValue != null ? ImageKey.Create(dbValue) : null);

        }
    }
}