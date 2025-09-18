using DBDAnalytics.Shared.Contracts.Responses;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Shared.Rarity
{
    public interface IRarityService :
        IRarityReadOnlyService,
        IBaseWriteApiService<RarityResponse, int>
    {
    }
}