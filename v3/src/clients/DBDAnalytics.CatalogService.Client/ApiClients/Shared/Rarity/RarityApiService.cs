using DBDAnalytics.Shared.Contracts.Responses;
using Shared.HttpClients;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Shared.Rarity
{
    public class RarityApiService(HttpClient httpClient)
        : BaseApiService<RarityResponse, int>(httpClient, "api/rarities"), IRarityService
    {
    }
}