using DBDAnalytics.Shared.Contracts.Responses.Maps;
using Shared.HttpClients;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Shared.WhoPlacedMap
{
    public class WhoPlacedMapApiService(HttpClient httpClient)
        : BaseApiService<WhoPlacedMapResponse, int>(httpClient, "api/who-placed-map"), IWhoPlacedMapService
    {
    }
}