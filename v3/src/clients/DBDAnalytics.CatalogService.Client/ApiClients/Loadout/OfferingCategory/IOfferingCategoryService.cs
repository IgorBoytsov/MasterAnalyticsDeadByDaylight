using DBDAnalytics.Shared.Contracts.Responses.Offering;
using Shared.HttpClients.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Loadout.OfferingCategory
{
    public interface IOfferingCategoryService :
        IOfferingCategoryReadOnlyService,
        IBaseWriteApiService<OfferingCategoryResponse, int>
    {
        Task<Result> UpdateNameAsync(int id, string newName);
    }
}