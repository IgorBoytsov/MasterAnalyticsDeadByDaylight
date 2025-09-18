using DBDAnalytics.CatalogService.Client.Models;
using DBDAnalytics.Shared.Domain.Results;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Loadout.ItemAddon
{
    public interface IItemAddonService : 
        IItemAddonReadOnlyService,
        IDeleteApiService<string>
    {
        Task<Result<string>> UpdateAsync(ClientUpdateItemAddonRequest request);
    }
}