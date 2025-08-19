using DBDAnalytics.Shared.Contracts.Requests.Shared;
using DBDAnalytics.Shared.Contracts.Responses.Maps;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.AddMap
{
    public sealed record CreateMapCommand(List<AddMapToMeasurementCommandData> Maps) : IRequest<Result<List<MapResponse>>>;

    public sealed record AddMapToMeasurementCommandData(Guid MeasurementId, int OldId, string Name, FileInput? Image, string SemanticImageName) :
        IHasName,
        IHasSemanticImageName;
}