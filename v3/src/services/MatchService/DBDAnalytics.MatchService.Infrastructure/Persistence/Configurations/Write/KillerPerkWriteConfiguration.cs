using DBDAnalytics.MatchService.Domain.Models;
using DBDAnalytics.MatchService.Domain.ValueObjects.KillerPerformance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.MatchService.Infrastructure.Persistence.Configurations.Write
{
    internal sealed class KillerPerkWriteConfiguration : IEntityTypeConfiguration<KillerPerk>
    {
        public void Configure(EntityTypeBuilder<KillerPerk> builder)
        {
            builder.ToTable("KillerPerks");

            builder.HasKey(p => new 
            { 
                p.KillerPerformanceId,
                p.PerkId 
            });

            builder.Property(p => p.KillerPerformanceId)
                .HasConversion(id => id.Value, value => KillerPerformanceId.From(value))
                .HasColumnName("KillerPerformanceId")
                .IsRequired(true);

            builder.Property(p => p.PerkId)
                .HasColumnName("PerkId")
                .IsRequired(true);
        }
    }
}