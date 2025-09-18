using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Loadout.ItemAddon
{
    public interface IItemAddonReadOnlyService : IBaseReadApiService<ItemAddonResponse, string>
    {
    }
}