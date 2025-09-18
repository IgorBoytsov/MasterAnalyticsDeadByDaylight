using DBDAnalytics.Shared.Contracts.Responses.Match;
using Shared.HttpClients;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Matches.GameEvent
{
    public class GameEventApiService(HttpClient httpClient) 
        : BaseApiService<GameEventResponse, int>(httpClient, "api/game-events"), IGameEventService
    {
    }
}