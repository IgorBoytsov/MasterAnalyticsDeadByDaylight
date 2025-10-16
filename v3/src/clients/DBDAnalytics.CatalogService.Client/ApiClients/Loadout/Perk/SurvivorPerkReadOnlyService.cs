 using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using Shared.HttpClients;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Loadout.Perk
{
    public sealed class SurvivorPerkReadOnlyService(HttpClient client)
        : BaseReadApiService<SurvivorPerkResponse, string>(client, "api/perks/survivors"), ISurvivorPerkReadOnlyService
    {
    }
}