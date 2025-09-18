using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.Survivor
{
    public interface ISurvivorReadOnlyService : IBaseReadApiService<SurvivorSoloResponse, string>
    {
    }
}