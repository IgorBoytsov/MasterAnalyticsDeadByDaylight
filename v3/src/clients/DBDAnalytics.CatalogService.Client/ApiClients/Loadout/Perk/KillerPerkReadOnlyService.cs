using DBDAnalytics.Shared.Contracts.Responses.Killers;
using Shared.HttpClients;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Loadout.Perk
{
    public sealed class KillerPerkReadOnlyService(HttpClient client) 
        : BaseReadApiService<KillerPerkResponse, string>(client, "api/perks/killers"), IKillerPerkReadOnlyService
    {
    }
}