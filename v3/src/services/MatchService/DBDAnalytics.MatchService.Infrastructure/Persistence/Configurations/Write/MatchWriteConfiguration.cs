using DBDAnalytics.MatchService.Domain.Models;
using DBDAnalytics.Shared.Domain.ValueObjects;
using DBDAnalytics.Shared.Domain.ValueObjects.Match;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.MatchService.Infrastructure.Persistence.Configurations.Write
{
    internal sealed class MatchWriteConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.ToTable("Matches");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasConversion(
                    id => id.Value,
                    dbValue => new MatchId(dbValue))
                .HasColumnName("Id")
                .ValueGeneratedNever();

            builder.Property(m => m.UserId)
                .HasConversion(
                    id => id.Value, 
                    dbValue => new UserId(dbValue))
                .HasColumnName("UserId")
                .IsRequired(true);

            builder.Property(m => m.GameModeId)
                .HasConversion(
                    id => id.Value, 
                    dbValue => new GameModeId(dbValue))
                .HasColumnName("GameModeId")
                .IsRequired(true);

            builder.Property(m => m.GameEventId)
                .HasConversion(
                    id => id.Value, 
                    dbValue => new GameEventId(dbValue))
                .HasColumnName("GameEventId")
                .IsRequired(true);

            builder.Property(m => m.MapId)
                .HasConversion(
                    id => id.Value, 
                    dbValue => new MapId(dbValue))
                .HasColumnName("MapId")
                .IsRequired(true);

            builder.Property(m => m.WhoPlacedMapId)
                .HasConversion(
                    id => id.Value, 
                    dbValue => new WhoPlacedMapId(dbValue))
                .HasColumnName("WhoPlacedMapId")
                .IsRequired(true);

            builder.Property(m => m.WhoPlacedMapId)
                .HasConversion(
                    id => id.Value, 
                    dbValue => new WhoPlacedMapId(dbValue))
                .HasColumnName("WhoPlacedMapId")
                .IsRequired(true);

            builder.Property(m => m.WhoPlacedMapWinId)
                .HasConversion(
                    id => id.Value, 
                    dbValue => new WhoPlacedMapId(dbValue))
                .HasColumnName("WhoPlacedMapWinId")
                .IsRequired(true);

            builder.Property(m => m.PatchId)
                .HasConversion(
                    id => id.Value,
                    dbValue => new PatchId(dbValue))
                .HasColumnName("PatchId")
                .IsRequired(true);

            builder.Property(m => m.CountKills)
                .HasConversion(
                    id => id.Value, 
                    dbValue => new CountKill(dbValue))
                .HasColumnName("CountKills")
                .IsRequired(true);

            builder.Property(m => m.CountHooks)
                .HasConversion(
                    id => id.Value, 
                    dbValue => new CountHook(dbValue))
                .HasColumnName("CountHooks")
                .IsRequired(true);

            builder.Property(m => m.CountRecentGenerators)
                .HasConversion(
                    id => id.Value, 
                    dbValue => new RecentGenerator(dbValue))
                .HasColumnName("CountRecentGenerators")
                .IsRequired(true);

            builder.Property(m => m.StartedAt)
                .HasConversion(
                    id => id.Value, 
                    dbValue => new MatchStartedAt(dbValue))
                .HasColumnName("StartedAt")
                .IsRequired(true);

            builder.Property(m => m.Duration)
                .HasConversion(
                    id => id.Value, 
                    dbValue => new MatchDuration(dbValue))
                .HasColumnName("Duration")
                .IsRequired(true);

            builder.Navigation(m => m.KillerPerformances).HasField("_killerPerformances").UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(m => m.KillerPerformances)
                .WithOne()
                .HasForeignKey(kp => kp.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(m => m.SurvivorPerformances).HasField("_survivorPerformances").UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(m => m.SurvivorPerformances)
                .WithOne()
                .HasForeignKey(kp => kp.MatchId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}