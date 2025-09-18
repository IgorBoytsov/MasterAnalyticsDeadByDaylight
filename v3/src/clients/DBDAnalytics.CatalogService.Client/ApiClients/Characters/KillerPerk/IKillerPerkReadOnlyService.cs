using DBDAnalytics.Shared.Contracts.Responses.Killers;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerPerk
{
    public interface IKillerPerkReadOnlyService : IBaseReadApiService<KillerPerkResponse, string>
    {
    }
}