using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Infrastructure.Repositories
{
    public sealed class GameEventRepository(IApplicationDbContext context)
        : BaseRepository<GameEvent, IApplicationDbContext>(context), IGameEventRepository
    {
        public async Task<GameEvent> Get(int Id) => await _context.GameEvents.FirstOrDefaultAsync(g => g.Id == Id);

        public async Task<bool> Exist(string name) => await _context.GameEvents.AnyAsync(x => x.Name == name);
    }
}