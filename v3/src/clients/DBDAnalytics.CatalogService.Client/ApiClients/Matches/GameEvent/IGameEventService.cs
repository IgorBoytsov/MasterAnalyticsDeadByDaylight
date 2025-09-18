using DBDAnalytics.Shared.Contracts.Responses.Match;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Matches.GameEvent
{
    public interface IGameEventService :
        IGameEventReadOnlyService,
        IBaseWriteApiService<GameEventResponse, int>
    {
    }
}