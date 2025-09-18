using DBDAnalytics.Shared.Contracts.Responses.Maps;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Shared.WhoPlacedMap
{
    public interface IWhoPlacedMapService :
        IWhoPlacedMapReadOnlyService,
        IBaseWriteApiService<WhoPlacedMapResponse, int>
    {
    }
}