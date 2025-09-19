using DBDAnalytics.CatalogService.Client.Models;
using Shared.HttpClients.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Matches.Map
{
    public interface IMapApiService : 
        IMapReadOnlyApiService,
        IDeleteApiService<string>
    {
        Task<Result<string>> UpdateAsync(ClientUpdateMapRequest request);
    }
}