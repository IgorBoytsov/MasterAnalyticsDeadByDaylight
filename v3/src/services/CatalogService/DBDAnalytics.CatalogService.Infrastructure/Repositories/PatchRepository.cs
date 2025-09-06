using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Patch;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Infrastructure.Repositories
{
    internal sealed class PatchRepository(IApplicationDbContext context)
        : BaseRepository<Patch, IApplicationDbContext>(context), IPatchRepository
    {
        public async Task<Patch> Get(int id) => await _context.Patches.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<bool> Exist(string name) => await _context.Patches.AnyAsync(p => p.Name == name);

        public async Task<bool> ExistDate(DateTime date)
        {
            var patchDate = PatchDate.Create(date);
            return await _context.Patches.AnyAsync(p => p.Date == patchDate);
        }
    }
}