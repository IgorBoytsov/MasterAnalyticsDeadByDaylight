using DBDAnalytics.MatchService.Domain.Models;
using DBDAnalytics.MatchService.Domain.ValueObjects.SurvivorPerformance;
using DBDAnalytics.Shared.Domain.ValueObjects;
using DBDAnalytics.Shared.Domain.ValueObjects.Match.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.MatchService.Infrastructure.Persistence.Configurations.Write
{
    internal sealed class SurvivorPerformanceWriteConfiguration : IEntityTypeConfiguration<SurvivorPerformance>
    {
        public void Configure(EntityTypeBuilder<SurvivorPerformance> builder)
        {
            builder.ToTable("SurvivorPerformances");

            builder.HasKey(kp => kp.Id);

            builder.Property(m => m.Id)
                .HasConversion(
                    id => id.Value,
                    dbValue => new SurvivorPerformanceId(dbValue))
                .HasColumnName("Id")
                .ValueGeneratedNever();

            builder.Property(m => m.MatchId)
                .HasConversion(
                    id => id.Value,
                    dbValue => new MatchId(dbValue))
                .HasColumnName("MatchId")
                .IsRequired(true);

            builder.Property(m => m.SurvivorId)
                .HasConversion(
                    id => id.Value,
                    dbValue => new SurvivorId(dbValue))
                .HasColumnName("SurvivorId")
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

            builder.Property(m => m.TypeDeathId)
                .HasConversion(
                    id => id.Value,
                    dbValue => new TypeDeathId(dbValue))
                .HasColumnName("TypeDeathId")
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

            builder.Navigation(p => p.Perks).HasField("_perks").UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(p => p.Perks)
                .WithOne()
                .HasForeignKey(p => p.SurvivorPerformanceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(p => p.Items).HasField("_items").UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(p => p.Items)
                .WithOne()
                .HasForeignKey(i => i.SurvivorPerformanceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}