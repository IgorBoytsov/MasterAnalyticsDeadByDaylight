using DBDAnalytics.MatchService.Domain.Models;
using DBDAnalytics.MatchService.Domain.ValueObjects.SurvivorPerformance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.MatchService.Infrastructure.Persistence.Configurations.Write
{
    internal sealed class SurvivorItemAddonWriteConfiguration : IEntityTypeConfiguration<SurvivorItemAddon>
    {
        public void Configure(EntityTypeBuilder<SurvivorItemAddon> builder)
        {
            builder.ToTable("SurvivorItemAddons");

            builder.HasKey(a => new 
            { 
                a.SurvivorItemId, 
                a.AddonId 
            });

            builder.Property(a => a.SurvivorItemId)
                .HasConversion(id => id.Value, value => SurvivorItemId.From(value))
                .HasColumnName("SurvivorItemId")
                .IsRequired(true);

            builder.Property(a => a.AddonId)
                .HasColumnName("AddonId")
                .IsRequired(true);
        }
    }
}