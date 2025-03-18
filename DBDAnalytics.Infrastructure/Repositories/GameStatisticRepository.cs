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

        /*--GET-------------------------------------------------------------------------------------------*/

        public async Task<List<GameStatisticKillerViewingDomain>> GetKillerViewsAsync()
        {
            using (var _context = _contextFactory())
            {
                var matches = await _context.GameStatistics
                    .Include(x => x.IdKillerNavigation.IdKillerNavigation)
                    .Include(x => x.IdMapNavigation)
                    .Where(x => x.IdKillerNavigation.IdAssociation == 1)
                    .OrderByDescending(x => x.DateTimeMatch)
                    .Take(100)
                    .Select(x => GameStatisticKillerViewingDomain.Create(
                        x.IdGameStatistic,
                        x.IdKiller,
                        x.IdKillerNavigation.IdKillerNavigation.KillerImage,
                        x.DateTimeMatch,
                        x.GameTimeMatch,
                        x.IdMapNavigation.MapName,
                        x.IdMapNavigation.MapImage,
                        x.CountKills,
                        x.CountHooks,
                        x.NumberRecentGenerators).GameStatisticKillerViewing)
                    .ToListAsync();

                return matches;
            }
        }

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
                        x.IdMapNavigation.MapImage,
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

        public async Task<List<GameStatisticSurvivorViewingDomain>> GetSurvivorViewsAsync()
        {
            using (var _context = _contextFactory())
            {
                var matches = await _context.GameStatistics
                    .Include(x => x.IdSurvivors1Navigation).ThenInclude(x => x.IdSurvivorNavigation)
                    .Include(x => x.IdSurvivors2Navigation).ThenInclude(x => x.IdSurvivorNavigation)
                    .Include(x => x.IdSurvivors3Navigation).ThenInclude(x => x.IdSurvivorNavigation)
                    .Include(x => x.IdSurvivors4Navigation).ThenInclude(x => x.IdSurvivorNavigation)
                    .Include(x => x.IdMapNavigation)
                    .Where(x => x.IdKillerNavigation.IdAssociation == 3)
                    .OrderByDescending(x => x.DateTimeMatch)
                    .Take(100)
                    .Select(x => GameStatisticSurvivorViewingDomain.Create(
                        x.IdGameStatistic,

                        (x.IdSurvivors1Navigation.IdAssociation == 1 ? x.IdSurvivors1 :
                         x.IdSurvivors2Navigation.IdAssociation == 1 ? x.IdSurvivors2 :
                         x.IdSurvivors3Navigation.IdAssociation == 1 ? x.IdSurvivors3 :
                         x.IdSurvivors4Navigation.IdAssociation == 1 ? x.IdSurvivors4 : 0),

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
                    .ToListAsync();

                return matches;
            }
        }

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
    }
}