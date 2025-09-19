using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.GameEvent;
using DBDAnalytics.CatalogService.Infrastructure.EF.Constants;
using DBDAnalytics.Shared.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Configurations
{
    internal class GameEventConfiguration : IEntityTypeConfiguration<GameEvent>
    {
        public void Configure(EntityTypeBuilder<GameEvent> builder)
        {
            builder.ToTable("GameEvents");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasColumnName("Id")
                .HasConversion(
                    modeId => modeId.Value,
                    dbValue => new GameEventId(dbValue))
                .ValueGeneratedOnAdd();

            builder.Property(c => c.OldId)
               .HasColumnName("OldId");

            builder.Property(m => m.Name)
                .HasColumnName("Name")
                .HasConversion(
                    name => name.Value,
                    dbValue => new GameEventName(dbValue))
                .HasMaxLength(GameEventName.MAX_LENGTH)
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .IsRequired();
        }
    }
}