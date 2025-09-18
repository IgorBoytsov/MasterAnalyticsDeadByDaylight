using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using Shared.HttpClients;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Shared.TypeDeath
{
    public class TypeDeathApiService(HttpClient httpClient)
        : BaseApiService<TypeDeathResponse, int>(httpClient, "api/type-deaths"), ITypeDeathService
    {
    }
}