using DBDAnalytics.Shared.Contracts.Responses;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Shared.Role
{
    public interface IRoleReadOnlyService : IBaseReadApiService<RoleResponse, int>
    {
    }
}