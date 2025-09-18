using DBDAnalytics.Shared.Contracts.Responses;
using Shared.HttpClients;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Shared.Role
{
    public class RoleApiService(HttpClient httpClient)
        : BaseApiService<RoleResponse, int>(httpClient, "api/roles"), IRoleService
    {
    }
}