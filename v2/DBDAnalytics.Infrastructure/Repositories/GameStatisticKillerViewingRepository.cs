using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class GameStatisticKillerViewingRepository(Func<DBDContext> context) : IGameStatisticKillerViewingRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        public async Task<GameStatisticKillerViewingDomain> GetKillerViewAsync(int idGameStatistic)
        {
            using (var _context = _contextFactory())
            {
                var match = await _context.GameStatistics
                    .Include(x => x.IdKillerNavigation.IdKillerNavigation)
                    .Include(x => x.IdMapNavigation)
                    .Where(x => x.IdGameStatistic == idGameStatistic)
                    .Select(x => GameStatisticKillerViewingDomain.Create(
                        x.IdGameStatistic,
                        x.IdKiller,
                        x.IdKillerNavigation.IdKillerNavigation.KillerImage,
                        x.DateTimeMatch,
                        x.GameTimeMatch,
                        x.IdMapNavigation.MapName,
                        x.CountKills,
                        x.CountHooks,
                        x.NumberRecentGenerators)
                    .GameStatisticKillerViewing).ToListAsync();

                return match.FirstOrDefault();
            }
        }

        public GameStatisticKillerViewingDomain GetKillerView(int idGameStatistic)
        {
            return Task.Run(() => GetKillerViewAsync(idGameStatistic)).Result;
        }

        public async Task<List<GameStatisticKillerViewingDomain>> GetKillerViewsFilteredAsync(GameStatisticKillerFilterDomain filter)
        {
            using (var _context = _contextFactory())
            {
                var query = _context.GameStatistics
                    .AsNoTracking()
                        .Include(x => x.IdKillerNavigation.IdKillerNavigation)
                        .Include(x => x.IdMapNavigation)
                            .AsSplitQuery()
                                .Where(x => x.IdKillerNavigation.IdAssociation == 1)
                                    .AsQueryable();

                if (filter.IdKiller.HasValue)
                    query = query.Where(x => x.IdKillerNavigation.IdKiller == filter.IdKiller.Value);

                if (filter.IdGameMode.HasValue)
                    query = query.Where(x => x.IdGameMode == filter.IdGameMode.Value);

                if (filter.IdGameEvent.HasValue)
                    query = query.Where(x => x.IdGameEvent == filter.IdGameEvent.Value);

                if (filter.IsConsiderDateTime == true)
                    query = query.Where(x => x.DateTimeMatch >= filter.EndTime && x.DateTimeMatch <= filter.StartTime);

                if (filter.IdPatch.HasValue)
                    query = query.Where(x => x.IdPatch == filter.IdPatch.Value);

                if (filter.IdMatchAttribute.HasValue)
                    query = query.Where(x => x.IdMatchAttribute == filter.IdMatchAttribute.Value);

                query = query.OrderByDescending(x => x.DateTimeMatch);

                var list = await query
                    .Select(item => GameStatisticKillerViewingDomain.Create(
                        item.IdGameStatistic,
                        item.IdKillerNavigation.IdKillerNavigation.IdKiller,
                        item.IdKillerNavigation.IdKillerNavigation.KillerImage,
                        item.DateTimeMatch,
                        item.GameTimeMatch,
                        item.IdMapNavigation.MapName,
                        item.CountKills,
                        item.CountHooks,
                        item.NumberRecentGenerators
                    ).GameStatisticKillerViewing)
                    .ToListAsync();

                return list;
            }
        }
    }
}