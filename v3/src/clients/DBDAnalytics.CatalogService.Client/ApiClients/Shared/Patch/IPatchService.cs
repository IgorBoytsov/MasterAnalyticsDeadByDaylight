using DBDAnalytics.Shared.Contracts.Responses;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Shared.Patch
{
    public interface IPatchService : 
        IPatchReadOnlyService, 
        IBaseWriteApiService<PatchResponse, int>
    {
    }
}