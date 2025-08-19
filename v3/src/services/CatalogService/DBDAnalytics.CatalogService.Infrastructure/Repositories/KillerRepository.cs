using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Infrastructure.Repositories
{
    internal sealed class KillerRepository(IApplicationDbContext context) 
        : BaseRepository<Killer, IApplicationDbContext>(context), IKillerRepository
    {
        public async Task<Killer> GetKiller(Guid id) 
            => await _context.Killers
                .Include(p => p.KillerPerks)
                .Include(a => a.KillerAddons)
                    .FirstOrDefaultAsync(k => k.Id == id);

        public async Task<bool> ExistName(string name) => await _context.Killers.AnyAsync(k => k.Name == name);

        public async Task<bool> ExistAddon(Guid idKiller, string addonName) => await _context.KillerAddons.AnyAsync(k => k.KillerId == idKiller && k.Name == addonName);

        public async Task<bool> ExistPerk(Guid idKiller, string perkName) => await _context.KillerPerks.AnyAsync(k => k.KillerId == idKiller && k.Name == perkName);
    }
}