using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Infrastructure.Repositories
{
    internal sealed class SurvivorPerkCategoryRepository(IApplicationDbContext context)
        : BaseRepository<SurvivorPerkCategory, IApplicationDbContext>(context), ISurvivorPerkCategoryRepository
    {
        public async Task<bool> Exist(string name) => await _context.SurvivorPerkCategories.AnyAsync(spc => spc.Name == name);
    }
}