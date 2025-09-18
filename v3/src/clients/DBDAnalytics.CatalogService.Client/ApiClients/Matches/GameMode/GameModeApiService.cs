using DBDAnalytics.Shared.Contracts.Responses.Match;
using Shared.HttpClients;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Matches.GameMode
{
    public class GameModeApiService(HttpClient httpClient)
        : BaseApiService<GameModeResponse, int>(httpClient, "api/game-modes"), IGameModeService
    {
    }
}