using DBDAnalytics.CatalogService.Client.Models;
using DBDAnalytics.Shared.Domain.Results;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Matches.Map
{
    public interface IMapApiService : 
        IMapReadOnlyApiService,
        IDeleteApiService<string>
    {
        Task<Result<string>> UpdateAsync(ClientUpdateMapRequest request);
    }
}