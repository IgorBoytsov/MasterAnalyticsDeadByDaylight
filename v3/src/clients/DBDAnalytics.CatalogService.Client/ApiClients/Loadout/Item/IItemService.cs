using DBDAnalytics.CatalogService.Client.Models;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using Shared.HttpClients.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Loadout.Item
{
    public interface IItemService : 
        IItemReadOnlyApiService,
        IDeleteApiService<string>
    {
        Task<Result<ItemResponse>> AddAsync(ClientAddItemRequest request);
        Task<Result<string>> UpdateAsync(ClientUpdateItemRequest request);
    }
}