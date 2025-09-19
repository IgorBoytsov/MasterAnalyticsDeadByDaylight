using DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions;
using DBDAnalytics.Shared.Contracts.Requests.Shared;
using DBDAnalytics.Shared.Contracts.Responses.Maps;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.Maps.Create
{
    public sealed record CreateMapCommand(List<AddMapToMeasurementCommandData> Maps) : IRequest<Result<List<MapResponse>>>;

    public sealed record AddMapToMeasurementCommandData(Guid MeasurementId, int OldId, string Name, FileInput? Image, string SemanticImageName) :
        IHasName,
        IMayHasFileInput,
        IHasSemanticImageName;
}