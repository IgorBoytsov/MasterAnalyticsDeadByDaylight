using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Patch;
using DBDAnalytics.CatalogService.Infrastructure.EF.Constants;
using DBDAnalytics.Shared.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Configurations
{
    internal sealed class PatchConfiguration : IEntityTypeConfiguration<Patch>
    {
        public void Configure(EntityTypeBuilder<Patch> builder)
        {
            builder.ToTable("Patches");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("Id")
                .HasConversion(
                    patchId => patchId.Value,
                    dbValue => new PatchId(dbValue))
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .HasColumnName("Name")
                .HasConversion(
                    name => name.Value,
                    dbValue => new PatchName(dbValue))
                .HasMaxLength(PatchName.MAX_LENGTH)
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .IsRequired();

            builder.Property(p => p.Date)
                .HasColumnName("Date")
                .HasConversion(
                    patchDate => patchDate.Value,
                    dbValue => new PatchDate(dbValue))
                .HasColumnType("timestamp with time zone")
                .IsRequired();
        }
    }
}