using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.GameMode;
using DBDAnalytics.CatalogService.Infrastructure.EF.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Configurations
{
    public sealed class GameModeConfiguration : IEntityTypeConfiguration<GameMode>
    {
        public void Configure(EntityTypeBuilder<GameMode> builder)
        {
            builder.ToTable("GameModes");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasColumnName("Id")
                .HasConversion(
                    modeId => modeId.Value,
                    dbValue => new GameModeId(dbValue))
                .ValueGeneratedOnAdd();

            builder.Property(c => c.OldId)
               .HasColumnName("OldId");

            builder.Property(m => m.Name)
                .HasColumnName("Name")
                .HasConversion(
                    name => name.Value,
                    dbValue => new GameModeName(dbValue))
                .HasMaxLength(GameModeName.MAX_LENGTH)
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .IsRequired();
        }
    }
}