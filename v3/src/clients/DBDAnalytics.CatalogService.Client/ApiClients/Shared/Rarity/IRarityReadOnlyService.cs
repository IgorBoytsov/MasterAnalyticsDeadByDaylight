using DBDAnalytics.Shared.Contracts.Responses;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Shared.Rarity
{
    public interface IRarityReadOnlyService : IBaseReadApiService<RarityResponse, int>
    {
    }
}