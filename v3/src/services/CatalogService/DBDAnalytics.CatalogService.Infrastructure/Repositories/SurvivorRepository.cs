using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Infrastructure.Repositories
{
    internal sealed class SurvivorRepository(IApplicationDbContext context)
        : BaseRepository<Survivor, IApplicationDbContext>(context), ISurvivorRepository
    {
        public async Task<Survivor> GetSurvivor(Guid id) => await _context.Survivors.Include(s => s.SurvivorPerks).FirstOrDefaultAsync(s => s.Id == id);

        public async Task<bool> Exist(string name) => await _context.Survivors.AnyAsync(s => s.Name == name);

    }
}