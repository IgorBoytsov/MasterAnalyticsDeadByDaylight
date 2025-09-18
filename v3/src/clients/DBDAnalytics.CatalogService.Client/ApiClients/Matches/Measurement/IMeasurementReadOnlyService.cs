using DBDAnalytics.Shared.Contracts.Responses.Maps;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Matches.Measurement
{
    public interface IMeasurementReadOnlyService : IBaseReadApiService<MeasurementSoloResponse, string>
    {
    }
}