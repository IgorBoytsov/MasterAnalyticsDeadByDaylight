using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Infrastructure.Repositories
{
    public sealed class GameModeRepository(IApplicationDbContext context)
        : BaseRepository<GameMode, IApplicationDbContext>(context), IGameModeRepository
    {
        public async Task<bool> Exist(string name) => await _context.GameModes.AnyAsync(x => x.Name == name);
    }
}