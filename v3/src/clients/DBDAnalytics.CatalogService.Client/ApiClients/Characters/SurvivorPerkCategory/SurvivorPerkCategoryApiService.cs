using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using DBDAnalytics.Shared.Domain.Results;
using Shared.HttpClients;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.SurvivorPerkCategory
{
    public class SurvivorPerkCategoryApiService(HttpClient httpClient)
        : BaseApiService<SurvivorPerkCategoryResponse, int>(httpClient, "api/survivor-perk-categories"), ISurvivorPerkCategoryService
    {
        public async Task<Result> UpdateNameAsync(int id, string newName)
        {
            var requestBody = new { NewName = newName };
            var result = await base.UpdateAsync(id, requestBody);
            return result;
        }
    }
}