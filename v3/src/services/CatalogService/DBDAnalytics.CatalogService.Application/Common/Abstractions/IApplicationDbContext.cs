using DBDAnalytics.CatalogService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Application.Common.Abstractions
{
    public interface IApplicationDbContext : IBaseDbContext
    {
        public DbSet<Rarity> Rarities { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<GameMode> GameModes { get; set; }
        public DbSet<GameEvent> GameEvents { get; set; }

        public DbSet<Map> Maps { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<WhoPlacedMap> WhoPlacedMaps { get; set; }

        public DbSet<TypeDeath> TypeDeaths { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<PlayerAssociation> PlayerAssociations { get; set; }

        public DbSet<Offering> Offerings { get; set; }
        public DbSet<OfferingCategory> OfferingCategories { get; set; }

        public DbSet<Killer> Killers { get; set; }
        public DbSet<KillerPerk> KillerPerks { get; set; }
        public DbSet<KillerAddon> KillerAddons { get; set; }
        public DbSet<KillerPerkCategory> KillerPerkCategories { get; set; }

        public DbSet<Survivor> Survivors {  get; set; }
        public DbSet<SurvivorPerk> SurvivorPerks { get; set; }
        public DbSet<SurvivorPerkCategory> SurvivorPerkCategories { get; set; }

        public DbSet<Item> Items { get; set; }
        public DbSet<ItemAddon> ItemsAddon { get; set; }
    }
}