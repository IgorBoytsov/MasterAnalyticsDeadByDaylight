using DBDAnalytics.Shared.Contracts.Responses.Offering;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Loadout.OfferingCategory
{
    public interface IOfferingCategoryReadOnlyService : IBaseReadApiService<OfferingCategoryResponse, int>
    {
    }
}