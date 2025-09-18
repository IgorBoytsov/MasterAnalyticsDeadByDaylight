using DBDAnalytics.Shared.Contracts.Responses.Killers;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerPerkCategory
{
    public interface IKillerPerkCategoryReadOnlyService : IBaseReadApiService<KillerPerkCategoryResponse, int>
    {
    }
}