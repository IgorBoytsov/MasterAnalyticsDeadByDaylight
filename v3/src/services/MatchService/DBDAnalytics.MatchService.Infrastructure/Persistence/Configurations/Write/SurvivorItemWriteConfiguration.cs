using DBDAnalytics.MatchService.Domain.Models;
using DBDAnalytics.MatchService.Domain.ValueObjects.SurvivorPerformance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.MatchService.Infrastructure.Persistence.Configurations.Write
{
    internal sealed class SurvivorItemWriteConfiguration : IEntityTypeConfiguration<SurvivorItem>
    {
        public void Configure(EntityTypeBuilder<SurvivorItem> builder)
        {
            builder.ToTable("SurvivorItems");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .HasConversion(id => id.Value, value => SurvivorItemId.From(value))
                .HasColumnName("Id")
                .ValueGeneratedNever();

            builder.Property(i => i.SurvivorPerformanceId)
                .HasConversion(id => id.Value, value => SurvivorPerformanceId.From(value))
                .HasColumnName("SurvivorPerformanceId")
                .IsRequired(true);

            builder.Property(i => i.ItemId)
                .HasColumnName("ItemId")
                .IsRequired(true);

            builder.Navigation(i => i.Addons).HasField("_addons").UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(i => i.Addons)
                   .WithOne()
                   .HasForeignKey(a => a.SurvivorItemId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}