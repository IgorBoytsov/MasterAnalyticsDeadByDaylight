using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Loadout.Perk
{
    public interface ISurvivorPerkReadOnlyService : IGetAllApiService<SurvivorPerkResponse>
    {
    }
}