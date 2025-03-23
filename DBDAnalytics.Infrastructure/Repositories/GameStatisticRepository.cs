using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class GameStatisticRepository(Func<DBDContext> context) : IGameStatisticRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        /*--CRUD------------------------------------------------------------------------------------------*/

        public int Create(int idKillerInfo,
                          int idFirstSurvivor, int idSecondSurvivor, int idThirdSurvivor, int idFourthSurvivor,
                          int idMap, int idWhoPlacedMap, int idWhoPlacedMapWin,
                          int idPatch, int idGameMode, int idGameEvent,
                          DateTime? dateTimeMatch, string? gameTimeMatch,
                          int countKills, int countHooks, int numberRecentGenerators,
                          string descriptionGame,
                          byte[]? resultMatch,
                          int idMatchAttribute)
        {
            using (var _context = _contextFactory())
            {
                var match = new GameStatistic
                {
                    IdKiller = idKillerInfo,
                    IdSurvivors1 = idFirstSurvivor,
                    IdSurvivors2 = idSecondSurvivor,
                    IdSurvivors3 = idThirdSurvivor,
                    IdSurvivors4 = idFourthSurvivor,
                    IdMap = idMap,
                    IdWhoPlacedMap = idWhoPlacedMap,
                    IdWhoPlacedMapWin = idWhoPlacedMapWin,
                    IdPatch = idPatch,
                    IdGameMode = idGameMode,
                    IdGameEvent = idGameEvent,
                    DateTimeMatch = dateTimeMatch,
                    GameTimeMatch = gameTimeMatch,
                    CountKills = countKills,
                    CountHooks = countHooks,
                    NumberRecentGenerators = numberRecentGenerators,
                    DescriptionGame = descriptionGame,
                    ResultMatch = resultMatch,
                    IdMatchAttribute = idMatchAttribute,
                };

                _context.GameStatistics.Add(match);
                _context.SaveChanges();

                int id = _context.GameStatistics
                            .OrderByDescending(x => x.IdGameStatistic)
                                .Select(x => x.IdGameStatistic)
                                    .FirstOrDefault();

                return id;
            }
        }

        /*--GET (KillerViewing)---------------------------------------------------------------------------*/
       
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

                //var list = new List<GameStatisticKillerViewingDomain>();

                //foreach (var item in await query.ToListAsync())
                //{
                //    list.Add(GameStatisticKillerViewingDomain.Create
                //        (
                //            item.IdGameStatistic,
                //            item.IdKillerNavigation.IdKillerNavigation.IdKiller,
                //            item.IdKillerNavigation.IdKillerNavigation.KillerImage,
                //            item.DateTimeMatch,
                //            item.GameTimeMatch,
                //            item.IdMapNavigation.MapName,
                //            item.CountKills,
                //            item.CountHooks,
                //            item.NumberRecentGenerators
                //        ).GameStatisticKillerViewing);
                //}

                //return list;
            }
        }

        /*--GET (SurvivorViewing)-------------------------------------------------------------------------*/

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

        /*--GET (Дата и время последнего матча)-----------------------------------------------------------*/

        public DateTime? GetLastDateMatch(bool isKillerMatch)
        {
            using (var _context = _contextFactory())
            {
                if (isKillerMatch)
                    return _context.GameStatistics
                        .Include(x => x.IdKillerNavigation)
                            .Where(x => x.IdKillerNavigation.IdAssociation == 1)
                                .OrderByDescending(x => x.DateTimeMatch)
                                    .FirstOrDefault()?.DateTimeMatch;
                else
                    return _context.GameStatistics
                        .Include(x => x.IdKillerNavigation)
                            .Where(x => x.IdKillerNavigation.IdAssociation == 3)
                                .OrderByDescending(x => x.DateTimeMatch)
                                    .FirstOrDefault()?.DateTimeMatch;
            }
        }

    }
}