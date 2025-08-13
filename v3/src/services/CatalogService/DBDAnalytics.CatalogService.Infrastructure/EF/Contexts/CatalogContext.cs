using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Contexts
{
    internal sealed class CatalogContext : DbContext, IApplicationDbContext
    {
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