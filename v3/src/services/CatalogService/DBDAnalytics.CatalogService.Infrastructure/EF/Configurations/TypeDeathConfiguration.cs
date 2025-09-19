using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.TypeDeath;
using DBDAnalytics.CatalogService.Infrastructure.EF.Constants;
using DBDAnalytics.Shared.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Configurations
{
    internal sealed class TypeDeathConfiguration : IEntityTypeConfiguration<TypeDeath>
    {
        public void Configure(EntityTypeBuilder<TypeDeath> builder)
        {
            builder.ToTable("TypeDeaths");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasColumnName("Id")
                .HasConversion(
                    typeDeathId => typeDeathId.Value,
                    dbValue => new TypeDeathId(dbValue))
                .ValueGeneratedOnAdd();

            builder.Property(t => t.OldId)
               .HasColumnName("OldId");

            builder.Property(t => t.Name)
                .HasColumnName("Name")
                .HasConversion(
                    name => name.Value,
                    dbValue => new TypeDeathName(dbValue))
                .HasMaxLength(TypeDeathName.MAX_LENGTH)
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .IsRequired();
        }
    }
}