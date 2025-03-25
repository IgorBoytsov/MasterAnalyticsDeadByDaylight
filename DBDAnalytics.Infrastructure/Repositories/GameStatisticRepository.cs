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