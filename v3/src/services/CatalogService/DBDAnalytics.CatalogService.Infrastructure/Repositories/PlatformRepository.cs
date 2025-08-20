using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Infrastructure.Repositories
{
    internal sealed class PlatformRepository(IApplicationDbContext context)
        : BaseRepository<Platform, IApplicationDbContext>(context), IPlatformRepository
    {
        public async Task<bool> Exist(string name) => await _context.Platforms.AnyAsync(p => p.Name == name);
    }
}