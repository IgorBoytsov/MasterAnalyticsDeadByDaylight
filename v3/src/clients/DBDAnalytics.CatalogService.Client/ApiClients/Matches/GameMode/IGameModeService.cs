using DBDAnalytics.Shared.Contracts.Responses.Match;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Matches.GameMode
{
    public interface IGameModeService : 
        IGameModeReadOnlyService,
        IBaseWriteApiService<GameModeResponse, int>
    {
    }
}