using DBDAnalytics.Shared.Contracts.Responses.Maps;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Shared.WhoPlacedMap
{
    public interface IWhoPlacedMapReadOnlyService : IBaseReadApiService<WhoPlacedMapResponse, int>
    {
    }
}