using DBDAnalytics.Shared.Contracts.Responses.Killers;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerAddon
{
    public interface IKillerAddonReadOnlyService : IBaseReadApiService<KillerAddonResponse, string>
    {
    }
}