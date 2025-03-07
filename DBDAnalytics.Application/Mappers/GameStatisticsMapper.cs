using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class GameStatisticsMapper
    {
        public static GameStatisticDTO ToDTO(this GameStatisticDomain gameStatistic)
        {
            return new GameStatisticDTO
            {
                IdGameStatistic = gameStatistic.IdGameStatistic,
                IdKiller = gameStatistic.IdKiller,
                IdMap = gameStatistic.IdMap,
                IdWhoPlacedMap = gameStatistic.IdWhoPlacedMap,
                IdWhoPlacedMapWin = gameStatistic.IdWhoPlacedMapWin,
                IdPatch = gameStatistic.IdPatch,
                IdGameEvent = gameStatistic.IdGameEvent,
                IdSurvivors1 = gameStatistic.IdSurvivor1,
                IdSurvivors2 = gameStatistic.IdSurvivor2,
                IdSurvivors3 = gameStatistic.IdSurvivor3,
                IdSurvivors4 = gameStatistic.IdSurvivor4,
                DateTimeMatch = gameStatistic.DateTimeMatch,
                GameTimeMatch = gameStatistic.GameTimeMatch,
                CountKills = gameStatistic.CountKills,
                CountHooks = gameStatistic.CountHooks,
                NumberRecentGenerators = gameStatistic.NumberRecentGenerators,
                DescriptionGame = gameStatistic.DescriptionGame,
                ResultMatch = gameStatistic.ResultMatch,
                IdGameMode = gameStatistic.IdGameMode,
                IdMatchAttribute = gameStatistic.IdMatchAttribute
            };
        }

        public static List<GameStatisticDTO> ToDTO(this IEnumerable<GameStatisticDomain> gameStatistics)
        {
           var list = new List<GameStatisticDTO>();

            foreach (var gameStatistic in gameStatistics)
            {
                list.Add(gameStatistic.ToDTO());
            }

            return list;
        }

        public static GameStatisticViewingDTO ToDTO(this GameStatisticViewingDomain gameStatistic)
        {
            return new GameStatisticViewingDTO
            {
                IdGameStatistic = gameStatistic.IdGameStatistic,
                IdKiller = gameStatistic.IdKiller,
                KillerImage = gameStatistic.KillerImage,
                DateMatch = gameStatistic.DateMatch,
                MatchTime = gameStatistic.MatchTime,
                MapName = gameStatistic.MapName,
                MapImage = gameStatistic.MapImage,
                CountKill = gameStatistic.CountKill,
                CountHook = gameStatistic.CountHook,
                CountRecentGenerator = gameStatistic.CountRecentGenerator,
                ResultMatch = gameStatistic.ResultMatch
            };
        }

        public static List<GameStatisticViewingDTO> ToDTO(this IEnumerable<GameStatisticViewingDomain> gameStatistics)
        {
            var list = new List<GameStatisticViewingDTO>();

            foreach (var gameStatistic in gameStatistics)
            {
                list.Add(gameStatistic.ToDTO());
            }

            return list;
        }
    }
}