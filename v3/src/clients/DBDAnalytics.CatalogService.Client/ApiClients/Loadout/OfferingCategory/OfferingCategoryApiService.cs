using DBDAnalytics.Shared.Contracts.Responses.Offering;
using DBDAnalytics.Shared.Domain.Results;
using Shared.HttpClients;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Loadout.OfferingCategory
{
    public sealed class OfferingCategoryApiService(HttpClient httpClient)
        : BaseApiService<OfferingCategoryResponse, int>(httpClient, "api/offering-categories"), IOfferingCategoryService
    {
        public async Task<Result> UpdateNameAsync(int id, string newName)
        {
            var requestBody = new { NewName = newName };
            var result = await UpdateAsync(id, requestBody);
            return result;
        }
    }
}