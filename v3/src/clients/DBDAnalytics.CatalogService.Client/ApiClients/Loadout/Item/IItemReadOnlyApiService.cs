using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Loadout.Item
{
    public interface IItemReadOnlyApiService : IBaseReadApiService<ItemSoloResponse, string>
    {
    }
}