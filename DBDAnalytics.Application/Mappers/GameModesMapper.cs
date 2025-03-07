using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class GameModesMapper
    {
        public static GameModeDTO ToDTO(this GameModeDomain gameMode)
        {
            return new GameModeDTO
            {
                IdGameMode = gameMode.IdGameMode,
                GameModeName = gameMode.GameModeName,
                GameModeDescription = gameMode.GameModeDescription,
            };
        }

        public static List<GameModeDTO> ToDTO(this IEnumerable<GameModeDomain> gameModes)
        {
            var list = new List<GameModeDTO>();

            foreach (var gameMode in gameModes)
            {
                //if (gameMode is null)
                //    continue;
                list.Add(gameMode.ToDTO());
            }

            return list;
        }
    }
}