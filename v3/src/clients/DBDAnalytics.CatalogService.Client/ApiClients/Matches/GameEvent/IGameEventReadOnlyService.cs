using DBDAnalytics.Shared.Contracts.Responses.Match;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Matches.GameEvent
{
    public interface IGameEventReadOnlyService : IBaseReadApiService<GameEventResponse, int>
    {
    }
}