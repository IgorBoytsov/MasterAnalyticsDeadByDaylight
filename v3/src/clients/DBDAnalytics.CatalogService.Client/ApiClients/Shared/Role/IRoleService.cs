using DBDAnalytics.Shared.Contracts.Responses;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Shared.Role
{
    public interface IRoleService :
        IRoleReadOnlyService,
        IBaseWriteApiService<RoleResponse, int>
    {
    }
}