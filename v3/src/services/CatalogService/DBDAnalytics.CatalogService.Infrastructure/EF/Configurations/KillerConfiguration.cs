using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Killer;
using DBDAnalytics.CatalogService.Infrastructure.EF.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Configurations
{
    internal sealed class KillerConfiguration : IEntityTypeConfiguration<Killer>
    {
        public void Configure(EntityTypeBuilder<Killer> builder)
        {
            builder.ToTable("Killers");

            builder.HasKey(k => k.Id);

            /*__Ids__*/
           
            builder.Property(k => k.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever();

            builder.Property(c => c.OldId)
                .HasColumnName("OldId");

            /*Название*/

            builder.Property(k => k.Name)
                .HasColumnName("Name")
                .HasConversion(
                    name => name.Value,
                    dbValue => new KillerName(dbValue))
                .HasMaxLength(KillerName.MAX_LENGTH)
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .IsRequired();

            /*Ключи изображений*/

            builder.Property(k => k.KillerImageKey)
                .HasColumnName("KillerImageKey")
                .HasConversion(
                    imageKey => imageKey != null ? imageKey.Value : null,
                    dbValue => dbValue != null ? ImageKey.Create(dbValue) : null);
            
            builder.Property(k => k.AbilityImageKey)
                .HasColumnName("AbilityImageKey")
                .HasConversion(
                    imageKey => imageKey != null ? imageKey.Value : null,
                    dbValue => dbValue != null ? ImageKey.Create(dbValue) : null);

            /*__Связи__*/

            builder.HasMany(k => k.KillerPerks)
                .WithOne(p => p.Killer)
                .HasForeignKey(p => p.KillerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(k => k.KillerPerks).HasField("_killerPerks").UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(k => k.KillerAddons)
                .WithOne(a => a.Killer)
                .HasForeignKey(a => a.KillerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(k => k.KillerAddons).HasField("_killerAddons").UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}