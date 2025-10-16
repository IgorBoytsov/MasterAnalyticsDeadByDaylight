using DBDAnalytics.Shared.Contracts.Responses.Killers;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Loadout.Perk
{
    public interface IKillerPerkReadOnlyService : IGetAllApiService<KillerPerkResponse>
    {
    }
}