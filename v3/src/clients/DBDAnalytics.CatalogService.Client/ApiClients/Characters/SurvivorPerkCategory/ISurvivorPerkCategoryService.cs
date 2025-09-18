using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using DBDAnalytics.Shared.Domain.Results;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.SurvivorPerkCategory
{
    public interface ISurvivorPerkCategoryService :
        ISurvivorPerkCategoryReadOnlyService,
        IBaseWriteApiService<SurvivorPerkCategoryResponse, int>
    {
        Task<Result> UpdateNameAsync(int id, string newName);
    }
}