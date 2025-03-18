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

        public static GameStatisticKillerViewingDTO ToDTO(this GameStatisticKillerViewingDomain gameStatistic)
        {
            return new GameStatisticKillerViewingDTO
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
            };
        }

        public static List<GameStatisticKillerViewingDTO> ToDTO(this IEnumerable<GameStatisticKillerViewingDomain> gameStatistics)
        {
            var list = new List<GameStatisticKillerViewingDTO>();

            foreach (var gameStatistic in gameStatistics)
            {
                list.Add(gameStatistic.ToDTO());
            }

            return list;
        }

        public static GameStatisticSurvivorViewingDTO ToDTO(this GameStatisticSurvivorViewingDomain gameStatistic)
        {
            return new GameStatisticSurvivorViewingDTO
            {
                IdGameStatistic = gameStatistic.IdGameStatistic,
                IdSurvivor = gameStatistic.IdSurvivor,
                SurvivorImage = gameStatistic.SurvivorImage,
                DateMatch = gameStatistic.DateMatch,
                MatchTime = gameStatistic.MatchTime,
                MapName = gameStatistic.MapName,
                CountKill = gameStatistic.CountKill,
                CountHook = gameStatistic.CountHook,
                CountRecentGenerator = gameStatistic.CountRecentGenerator,
            };
        }

        public static List<GameStatisticSurvivorViewingDTO> ToDTO(this IEnumerable<GameStatisticSurvivorViewingDomain> gameStatistics)
        {
            var list = new List<GameStatisticSurvivorViewingDTO>();

            foreach (var gameStatistic in gameStatistics)
            {
                list.Add(gameStatistic.ToDTO());
            }

            return list;
        }
    }
}