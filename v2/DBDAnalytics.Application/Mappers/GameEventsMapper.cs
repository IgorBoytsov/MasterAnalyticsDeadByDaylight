using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class GameEventsMapper
    {
        public static GameEventDTO ToDTO(this GameEventDomain gameEvent)
        {
            return new GameEventDTO
            {
                IdGameEvent = gameEvent.IdGameEvent,
                GameEventName = gameEvent.GameEventName,
                GameEventDescription = gameEvent.GameEventDescription,
            };
        }

        public static List<GameEventDTO> ToDTO(this IEnumerable<GameEventDomain> gameEvents)
        {
            var list = new List<GameEventDTO>();

            foreach (var gameEvent in gameEvents)
            {
                list.Add(gameEvent.ToDTO());
            }

            return list;
        }
    }
}