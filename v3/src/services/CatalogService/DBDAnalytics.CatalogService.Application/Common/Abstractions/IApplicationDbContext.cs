using DBDAnalytics.CatalogService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Application.Common.Abstractions
{
    public interface IApplicationDbContext : IBaseDbContext
    {
        public DbSet<Killer> Killers { get; set; }
        public DbSet<KillerPerk> KillerPerks { get; set; }
        public DbSet<KillerAddon> KillerAddons { get; set; }
        public DbSet<KillerPerkCategory> KillerPerkCategories { get; set; }
    }
}