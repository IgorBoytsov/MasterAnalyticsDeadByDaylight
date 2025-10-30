using DBDAnalytics.MatchService.Domain.Models;
using DBDAnalytics.MatchService.Domain.ValueObjects.SurvivorPerformance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.MatchService.Infrastructure.Persistence.Configurations.Write
{
    internal sealed class SurvivorPerkWriteConfiguration : IEntityTypeConfiguration<SurvivorPerk>
    {
        public void Configure(EntityTypeBuilder<SurvivorPerk> builder)
        {
            builder.ToTable("SurvivorPerks");

            builder.HasKey(p => new 
            {
                p.SurvivorPerformanceId,
                p.PerkId 
            });

            builder.Property(p => p.SurvivorPerformanceId)
                .HasConversion(id => id.Value, value => SurvivorPerformanceId.From(value))
                .HasColumnName("SurvivorPerformanceId")
                .IsRequired(true);

            builder.Property(p => p.PerkId)
                .HasColumnName("PerkId");
        }
    }
}