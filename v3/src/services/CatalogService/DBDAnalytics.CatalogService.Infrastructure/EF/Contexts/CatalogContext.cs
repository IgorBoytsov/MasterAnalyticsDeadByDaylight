using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Contexts
{
    internal sealed class CatalogContext : DbContext, IApplicationDbContext
    {
        public DbSet<Rarity> Rarities { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<GameMode> GameModes { get; set; }
        public DbSet<GameEvent> GameEvents { get; set; }

        public DbSet<Map> Maps { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<WhoPlacedMap> WhoPlacedMaps { get; set; }

        public DbSet<Killer> Killers { get; set; }
        public DbSet<KillerPerk> KillerPerks { get; set; }
        public DbSet<KillerAddon> KillerAddons { get; set; }
        public DbSet<KillerPerkCategory> KillerPerkCategories { get; set; }

        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}