using DBDAnalytics.Shared.Contracts.Responses.Killers;
using Shared.HttpClients;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerPerkCategory
{
    public class KillerPerkCategoryApiService(HttpClient httpClient)
        : BaseApiService<KillerPerkCategoryResponse, int>(httpClient, "api/killer-perk-categories"), IKillerPerkCategoryService
    {
    }
}