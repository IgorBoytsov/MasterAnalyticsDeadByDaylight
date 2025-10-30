using DBDAnalytics.MatchService.Domain.Models;
using DBDAnalytics.MatchService.Domain.ValueObjects.KillerPerformance;
using DBDAnalytics.Shared.Domain.ValueObjects;
using DBDAnalytics.Shared.Domain.ValueObjects.Match.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.MatchService.Infrastructure.Persistence.Configurations.Write
{
    internal sealed class KillerPerformanceWriteConfiguration : IEntityTypeConfiguration<KillerPerformance>
    {
        public void Configure(EntityTypeBuilder<KillerPerformance> builder)
        {
            builder.ToTable("KillerPerformances");

            builder.HasKey(kp =>  kp.Id);

            builder.Property(m => m.Id)
                .HasConversion(
                    id => id.Value,
                    dbValue => new KillerPerformanceId(dbValue))
                .HasColumnName("Id")
                .ValueGeneratedNever();

            builder.Property(m => m.MatchId)
                .HasConversion(
                    id => id.Value,
                    dbValue => new MatchId(dbValue))
                .HasColumnName("MatchId")
                .IsRequired(true);

            builder.Property(m => m.KillerId)
                .HasConversion(
                    id => id.Value,
                    dbValue => new KillerId(dbValue))
                .HasColumnName("KillerId")
                .IsRequired(true);

            builder.Property(m => m.OfferingId)
                .HasConversion(
                    id => id.Value,
                    dbValue => new OfferingId(dbValue))
                .HasColumnName("OfferingId")
                .IsRequired(true);

            builder.Property(m => m.AssociationId)
                .HasConversion(
                    id => id.Value,
                    dbValue => new PlayerAssociationId(dbValue))
                .HasColumnName("PlayerAssociationId")
                .IsRequired(true);

            builder.Property(m => m.PlatformId)
                .HasConversion(
                    id => id.Value,
                    dbValue => new PlatformId(dbValue))
                .HasColumnName("PlatformId")
                .IsRequired(true);

            builder.Property(m => m.Prestige)
                .HasConversion(
                    id => id.Value,
                    dbValue => new Prestige(dbValue))
                .HasColumnName("Prestige")
                .IsRequired(true);

            builder.Property(m => m.IsBot)
                .HasColumnName("IsBot")
                .IsRequired(true);

            builder.Property(m => m.IsAnonymousMode)
                .HasColumnName("IsAnonymousMode")
                .IsRequired(true);

            builder.Property(m => m.Score)
                .HasConversion(
                    id => id.Value,
                    dbValue => new Score(dbValue))
                .HasColumnName("Score")
                .IsRequired(true);

            builder.Property(m => m.Experience)
                .HasConversion(
                    id => id.Value,
                    dbValue => new Experience(dbValue))
                .HasColumnName("Experience")
                .IsRequired(true);

            builder.Property(m => m.NumberBloodDrops)
                .HasConversion(
                    id => id.Value,
                    dbValue => new Blood(dbValue))
                .HasColumnName("NumberBloodDrops")
                .IsRequired(true);

            builder.Navigation(kp => kp.Perks).HasField("_perks").UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(kp => kp.Perks)
                   .WithOne()
                   .HasForeignKey(p => p.KillerPerformanceId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(kp => kp.Addons).HasField("_addons").UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(kp => kp.Addons)
                   .WithOne()
                   .HasForeignKey(p => p.KillerPerformanceId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}