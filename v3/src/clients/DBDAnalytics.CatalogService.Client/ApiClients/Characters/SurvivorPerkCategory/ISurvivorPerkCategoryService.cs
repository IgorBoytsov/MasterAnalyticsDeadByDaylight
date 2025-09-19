using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using Shared.HttpClients.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.SurvivorPerkCategory
{
    public interface ISurvivorPerkCategoryService :
        ISurvivorPerkCategoryReadOnlyService,
        IBaseWriteApiService<SurvivorPerkCategoryResponse, int>
    {
        Task<Result> UpdateNameAsync(int id, string newName);
    }
}