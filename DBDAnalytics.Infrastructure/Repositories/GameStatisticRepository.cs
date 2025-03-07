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

        public async Task<GameStatisticViewingDomain?> GetAsync(int idGameStatistic)
        {
            using (var _context = _contextFactory())
            {
                var match = await _context.GameStatistics
                    .Include(x => x.IdKillerNavigation.IdKillerNavigation)
                    .Include(x => x.IdMapNavigation)
                    .Where(x => x.IdGameStatistic == idGameStatistic)
                    .Select(x => GameStatisticViewingDomain.Create(
                        x.IdGameStatistic,
                        x.IdKiller,
                        x.IdKillerNavigation.IdKillerNavigation.KillerImage,
                        x.DateTimeMatch,
                        x.GameTimeMatch,
                        x.IdMapNavigation.MapName,
                        x.IdMapNavigation.MapImage,
                        x.CountKills,
                        x.CountHooks,
                        x.NumberRecentGenerators,
                        x.ResultMatch).GameStatisticViewingDTO).ToListAsync();

                if (match == null)
                {
                    Console.WriteLine($"Не удалось получить GameStatistics для Id: {idGameStatistic}");
                    return null;
                }

                return match.FirstOrDefault();
            }
        }

        public GameStatisticViewingDomain? Get(int idGameStatistic)
        {
            return Task.Run(() => GetAsync(idGameStatistic)).Result;
        }

        public async Task<List<GameStatisticViewingDomain>> GetViewsAsync()
        {
            using (var _context = _contextFactory())
            {
                var matches = await _context.GameStatistics
                    .Include(x => x.IdKillerNavigation.IdKillerNavigation)
                    .Include(x => x.IdMapNavigation)
                    .Select(x => GameStatisticViewingDomain.Create(
                        x.IdGameStatistic, 
                        x.IdKiller, 
                        x.IdKillerNavigation.IdKillerNavigation.KillerImage,
                        x.DateTimeMatch, 
                        x.GameTimeMatch,
                        x.IdMapNavigation.MapName, 
                        x.IdMapNavigation.MapImage,
                        x.CountKills,
                        x.CountHooks, 
                        x.NumberRecentGenerators,
                        x.ResultMatch).GameStatisticViewingDTO)
                    .ToListAsync();

                return matches;
            }
        }
    }
}