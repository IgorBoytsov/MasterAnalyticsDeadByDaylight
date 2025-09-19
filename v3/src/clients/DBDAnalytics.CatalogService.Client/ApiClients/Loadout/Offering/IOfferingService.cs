using DBDAnalytics.Shared.Contracts.Responses.Offering;
using Shared.HttpClients.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Loadout.Offering
{
    public interface IOfferingService :
        IOfferingReadOnlyService,
        IDeleteApiService<string>
    {
        Task<Result<OfferingResponse>> AddAsync(
            int oldId,
            string name,
            string semanticName,
            string imagePath,
            int roleId,
            int? rarityId,
            int? categoryId);

        Task<Result<string>> UpdateAsync(
            string Id,
            string name,
            string semanticName,
            string imagePath,
            int roleId,
            int? rarityId,
            int? categoryId);
    }
}