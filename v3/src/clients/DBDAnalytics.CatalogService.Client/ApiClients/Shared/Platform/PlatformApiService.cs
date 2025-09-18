using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using Shared.HttpClients;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Shared.Platform
{
    public class PlatformApiService(HttpClient httpClient)
        : BaseApiService<PlatformResponse, int>(httpClient, "api/platforms"), IPlatformService
    {
    }
}