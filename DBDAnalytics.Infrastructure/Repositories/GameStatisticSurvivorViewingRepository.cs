using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class GameStatisticSurvivorViewingRepository(Func<DBDContext> context) : IGameStatisticSurvivorViewingRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        public async Task<GameStatisticSurvivorViewingDomain> GetSurvivorViewAsync(int idGameStatistic)
        {
            using (var _context = _contextFactory())
            {
                var matches = await _context.GameStatistics
                    .Include(x => x.IdSurvivors1Navigation).ThenInclude(x => x.IdSurvivorNavigation)
                    .Include(x => x.IdSurvivors2Navigation).ThenInclude(x => x.IdSurvivorNavigation)
                    .Include(x => x.IdSurvivors3Navigation).ThenInclude(x => x.IdSurvivorNavigation)
                    .Include(x => x.IdSurvivors4Navigation).ThenInclude(x => x.IdSurvivorNavigation)
                    .Include(x => x.IdMapNavigation)
                    .OrderByDescending(x => x.DateTimeMatch)
                    .Where(x => x.IdGameStatistic == idGameStatistic)
                    .Select(x => GameStatisticSurvivorViewingDomain.Create(
                        x.IdGameStatistic,

                        (x.IdSurvivors1Navigation.IdAssociation == 1 ? x.IdSurvivors1 :
                         x.IdSurvivors2Navigation.IdAssociation == 1 ? x.IdSurvivors2 :
                         x.IdSurvivors3Navigation.IdAssociation == 1 ? x.IdSurvivors3 :
                         x.IdSurvivors4Navigation.IdAssociation == 1 ? x.IdSurvivors4 : 0),

                        (x.IdSurvivors1Navigation.IdAssociation == 1 ? x.IdSurvivors1Navigation.IdTypeDeath :
                         x.IdSurvivors2Navigation.IdAssociation == 1 ? x.IdSurvivors1Navigation.IdTypeDeath :
                         x.IdSurvivors3Navigation.IdAssociation == 1 ? x.IdSurvivors1Navigation.IdTypeDeath :
                         x.IdSurvivors4Navigation.IdAssociation == 1 ? x.IdSurvivors1Navigation.IdTypeDeath : 0),

                        (x.IdSurvivors1Navigation.IdAssociation == 1 ? x.IdSurvivors1Navigation.IdSurvivorNavigation.SurvivorImage :
                         x.IdSurvivors2Navigation.IdAssociation == 1 ? x.IdSurvivors2Navigation.IdSurvivorNavigation.SurvivorImage :
                         x.IdSurvivors3Navigation.IdAssociation == 1 ? x.IdSurvivors3Navigation.IdSurvivorNavigation.SurvivorImage :
                         x.IdSurvivors4Navigation.IdAssociation == 1 ? x.IdSurvivors4Navigation.IdSurvivorNavigation.SurvivorImage : null),

                        x.DateTimeMatch,
                        x.GameTimeMatch,
                        x.IdMapNavigation.MapName,
                        x.CountKills,
                        x.CountHooks,
                        x.NumberRecentGenerators)
                    .GameStatisticSurvivorViewingDomain)
                    .FirstOrDefaultAsync(x => x.IdGameStatistic == idGameStatistic);

                return matches;
            }
        }

        public GameStatisticSurvivorViewingDomain GetSurvivorView(int idGameStatistic)
        {
            return Task.Run(() => GetSurvivorViewAsync(idGameStatistic)).Result;
        }

        public async Task<List<GameStatisticSurvivorViewingDomain>> GetSurvivorViewsAsync(GameStatisticSurvivorFilterDomain filter)
        {
            using (var _context = _contextFactory())
            {
                var query = _context.GameStatistics
                     .AsNoTracking()
                        .Include(x => x.IdSurvivors1Navigation).ThenInclude(x => x.IdSurvivorNavigation)
                        .Include(x => x.IdSurvivors2Navigation).ThenInclude(x => x.IdSurvivorNavigation)
                        .Include(x => x.IdSurvivors3Navigation).ThenInclude(x => x.IdSurvivorNavigation)
                        .Include(x => x.IdSurvivors4Navigation).ThenInclude(x => x.IdSurvivorNavigation)
                        .Include(x => x.IdMapNavigation)
                        .Include(x => x.IdKillerNavigation.IdKillerNavigation)
                            .AsSplitQuery()
                                .Where(x => x.IdKillerNavigation.IdAssociation != 1)
                                    .AsQueryable();

                if (filter.IdOpponentKiller.HasValue)
                    query = query.Where(x => x.IdKillerNavigation.IdKiller == filter.IdOpponentKiller.Value);

                if (filter.IdSurvivor.HasValue)
                {
                    query = query.Where(x => x.IdSurvivors1Navigation.IdSurvivor == filter.IdSurvivor.Value ||
                                             x.IdSurvivors2Navigation.IdSurvivor == filter.IdSurvivor.Value ||
                                             x.IdSurvivors3Navigation.IdSurvivor == filter.IdSurvivor.Value ||
                                             x.IdSurvivors4Navigation.IdSurvivor == filter.IdSurvivor.Value);
                }

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
                    .Select(x => GameStatisticSurvivorViewingDomain.Create(
                        x.IdGameStatistic,

                        filter.IdSurvivor.HasValue
                        ? (x.IdSurvivors1Navigation.IdSurvivor == filter.IdSurvivor ? x.IdSurvivors1 :
                           x.IdSurvivors2Navigation.IdSurvivor == filter.IdSurvivor ? x.IdSurvivors2 :
                           x.IdSurvivors3Navigation.IdSurvivor == filter.IdSurvivor ? x.IdSurvivors3 :
                           x.IdSurvivors4Navigation.IdSurvivor == filter.IdSurvivor ? x.IdSurvivors4 : 0)
                        : (x.IdSurvivors1Navigation.IdAssociation == 1 ? x.IdSurvivors1 :
                           x.IdSurvivors2Navigation.IdAssociation == 1 ? x.IdSurvivors2 :
                           x.IdSurvivors3Navigation.IdAssociation == 1 ? x.IdSurvivors3 :
                           x.IdSurvivors4Navigation.IdAssociation == 1 ? x.IdSurvivors4 : 0),

                        filter.IdSurvivor.HasValue
                        ? (x.IdSurvivors1Navigation.IdSurvivor == filter.IdSurvivor ? x.IdSurvivors1Navigation.IdTypeDeath :
                           x.IdSurvivors2Navigation.IdSurvivor == filter.IdSurvivor ? x.IdSurvivors2Navigation.IdTypeDeath :
                           x.IdSurvivors3Navigation.IdSurvivor == filter.IdSurvivor ? x.IdSurvivors3Navigation.IdTypeDeath :
                           x.IdSurvivors4Navigation.IdSurvivor == filter.IdSurvivor ? x.IdSurvivors4Navigation.IdTypeDeath : 0)
                        : (x.IdSurvivors1Navigation.IdAssociation == 1 ? x.IdSurvivors1Navigation.IdTypeDeath :
                           x.IdSurvivors2Navigation.IdAssociation == 1 ? x.IdSurvivors2Navigation.IdTypeDeath :
                           x.IdSurvivors3Navigation.IdAssociation == 1 ? x.IdSurvivors3Navigation.IdTypeDeath :
                           x.IdSurvivors4Navigation.IdAssociation == 1 ? x.IdSurvivors4Navigation.IdTypeDeath : 0),

                        filter.IdSurvivor.HasValue
                        ? (x.IdSurvivors1Navigation.IdSurvivor == filter.IdSurvivor ? x.IdSurvivors1Navigation.IdSurvivorNavigation.SurvivorImage :
                           x.IdSurvivors2Navigation.IdSurvivor == filter.IdSurvivor ? x.IdSurvivors2Navigation.IdSurvivorNavigation.SurvivorImage :
                           x.IdSurvivors3Navigation.IdSurvivor == filter.IdSurvivor ? x.IdSurvivors3Navigation.IdSurvivorNavigation.SurvivorImage :
                           x.IdSurvivors4Navigation.IdSurvivor == filter.IdSurvivor ? x.IdSurvivors4Navigation.IdSurvivorNavigation.SurvivorImage : null)
                        : (x.IdSurvivors1Navigation.IdAssociation == 1 ? x.IdSurvivors1Navigation.IdSurvivorNavigation.SurvivorImage :
                           x.IdSurvivors2Navigation.IdAssociation == 1 ? x.IdSurvivors2Navigation.IdSurvivorNavigation.SurvivorImage :
                           x.IdSurvivors3Navigation.IdAssociation == 1 ? x.IdSurvivors3Navigation.IdSurvivorNavigation.SurvivorImage :
                           x.IdSurvivors4Navigation.IdAssociation == 1 ? x.IdSurvivors4Navigation.IdSurvivorNavigation.SurvivorImage : null),

                         x.DateTimeMatch,
                         x.GameTimeMatch,
                         x.IdMapNavigation.MapName,
                         x.CountKills,
                         x.CountHooks,
                         x.NumberRecentGenerators
                     ).GameStatisticSurvivorViewingDomain)
                    .ToListAsync();

                return list;
            }
        }
    }
}