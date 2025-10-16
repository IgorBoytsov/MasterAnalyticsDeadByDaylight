using DBDAnalytics.Shared.Contracts.Responses.Maps;
using Shared.HttpClients;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Matches.Map
{
    public sealed class MapApiService(HttpClient client)
        : BaseReadApiService<MapResponse, string>(client, "api/maps"), IMapReadOnlyService
    {
    }
}