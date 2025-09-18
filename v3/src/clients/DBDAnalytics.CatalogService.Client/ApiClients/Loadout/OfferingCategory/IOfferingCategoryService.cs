using DBDAnalytics.Shared.Contracts.Responses.Offering;
using DBDAnalytics.Shared.Domain.Results;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Loadout.OfferingCategory
{
    public interface IOfferingCategoryService :
        IOfferingCategoryReadOnlyService,
        IBaseWriteApiService<OfferingCategoryResponse, int>
    {
        Task<Result> UpdateNameAsync(int id, string newName);
    }
}