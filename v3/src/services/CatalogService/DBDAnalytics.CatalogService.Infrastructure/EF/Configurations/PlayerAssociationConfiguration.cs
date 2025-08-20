using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.PlayerAssociation;
using DBDAnalytics.CatalogService.Infrastructure.EF.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Configurations
{
    internal sealed class PlayerAssociationConfiguration : IEntityTypeConfiguration<PlayerAssociation>
    {
        public void Configure(EntityTypeBuilder<PlayerAssociation> builder)
        {
            builder.ToTable("PlayerAssociations");

            builder.HasKey(p => p.Id);

            builder.Property(r => r.Id)
                .HasColumnName("Id")
                .HasConversion(
                    playerAssociationId => playerAssociationId.Value,
                    dbValue => new PlayerAssociationId(dbValue))
                .ValueGeneratedOnAdd();

            builder.Property(p => p.OldId)
               .HasColumnName("OldId");

            builder.Property(p => p.Name)
                .HasColumnName("Name")
                .HasConversion(
                    name => name.Value,
                    dbValue => new PlayerAssociationName(dbValue))
                .HasMaxLength(PlayerAssociationName.MAX_LENGTH)
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .IsRequired();
        }
    }
}