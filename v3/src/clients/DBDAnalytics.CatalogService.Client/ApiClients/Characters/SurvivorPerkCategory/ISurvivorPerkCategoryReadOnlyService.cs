using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.SurvivorPerkCategory
{
    public interface ISurvivorPerkCategoryReadOnlyService : IBaseReadApiService<SurvivorPerkCategoryResponse, int>
    {
    }
}