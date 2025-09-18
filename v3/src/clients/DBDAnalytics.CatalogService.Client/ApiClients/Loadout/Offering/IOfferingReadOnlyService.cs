using DBDAnalytics.Shared.Contracts.Responses.Offering;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Loadout.Offering
{
    public interface IOfferingReadOnlyService : IBaseReadApiService<OfferingResponse, string>
    {
    }
}