using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Infrastructure.Repositories
{
    internal sealed class KillerPerkCategoryRepository(IApplicationDbContext context)
        : BaseRepository<KillerPerkCategory, IApplicationDbContext>(context), IKillerPerkCategoryRepository
    {
        public async Task<KillerPerkCategory> Get(int id) => await _context.KillerPerkCategories.FirstOrDefaultAsync(kpc => kpc.Id == id);

        public async Task<bool> Exist(string name) => await _context.KillerPerkCategories.AnyAsync(c => c.Name == name);
    }
}