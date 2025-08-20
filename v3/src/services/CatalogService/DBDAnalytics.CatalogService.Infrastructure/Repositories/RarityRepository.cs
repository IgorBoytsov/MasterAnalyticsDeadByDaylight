using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Infrastructure.Repositories
{
    internal sealed class RarityRepository(IApplicationDbContext context) : BaseRepository<Rarity, IApplicationDbContext>(context), IRarityRepository
    {
        public async Task<bool> Exist(string name) => await _context.Rarities.AnyAsync(r => r.Name == name);
    }
}