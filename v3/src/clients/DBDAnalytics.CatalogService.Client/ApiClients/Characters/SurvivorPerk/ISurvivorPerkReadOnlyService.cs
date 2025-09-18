using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.SurvivorPerk
{
    public interface ISurvivorPerkReadOnlyService : IBaseReadApiService<SurvivorPerkResponse, string>
    {
    }
}