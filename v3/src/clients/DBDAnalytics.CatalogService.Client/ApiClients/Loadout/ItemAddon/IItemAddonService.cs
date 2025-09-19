using DBDAnalytics.CatalogService.Client.Models;
using Shared.HttpClients.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Loadout.ItemAddon
{
    public interface IItemAddonService : 
        IItemAddonReadOnlyService,
        IDeleteApiService<string>
    {
        Task<Result<string>> UpdateAsync(ClientUpdateItemAddonRequest request);
    }
}