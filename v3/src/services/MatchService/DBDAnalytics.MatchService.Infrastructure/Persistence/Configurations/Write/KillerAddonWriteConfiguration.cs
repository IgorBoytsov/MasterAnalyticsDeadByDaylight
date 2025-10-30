using DBDAnalytics.MatchService.Domain.Models;
using DBDAnalytics.MatchService.Domain.ValueObjects.KillerPerformance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.MatchService.Infrastructure.Persistence.Configurations.Write
{
    internal sealed class KillerAddonWriteConfiguration : IEntityTypeConfiguration<KillerAddon>
    {
        public void Configure(EntityTypeBuilder<KillerAddon> builder)
        {
            builder.ToTable("KillerAddons");

            builder.HasKey(a => new 
            { 
                a.KillerPerformanceId, 
                a.AddonId 
            });

            builder.Property(a => a.KillerPerformanceId)
                .HasConversion(id => id.Value, value => KillerPerformanceId.From(value))
                .HasColumnName("KillerPerformanceId")
                .IsRequired(true);

            builder.Property(a => a.AddonId)
                .HasColumnName("AddonId")
                .IsRequired(true);
        }
    }
}