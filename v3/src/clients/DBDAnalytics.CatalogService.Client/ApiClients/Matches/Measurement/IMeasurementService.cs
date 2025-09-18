using DBDAnalytics.CatalogService.Client.Models;
using DBDAnalytics.Shared.Contracts.Responses.Maps;
using DBDAnalytics.Shared.Domain.Results;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.CatalogService.Client.ApiClients.Matches.Measurement
{
    public interface IMeasurementService :
        IMeasurementReadOnlyService,
        IUpdateApiService<string>,
        IDeleteApiService<string>
    {
        Task<Result<MeasurementSoloResponse>> AddAsync(ClientAddMeasurementRequest request);
    }
}