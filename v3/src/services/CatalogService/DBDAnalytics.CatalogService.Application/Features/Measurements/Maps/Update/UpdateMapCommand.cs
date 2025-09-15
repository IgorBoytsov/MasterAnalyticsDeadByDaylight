using DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions;
using DBDAnalytics.Shared.Contracts.Requests.Shared;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.Maps.Update
{
    public sealed record UpdateMapCommand(Guid MeasurementId, Guid MapId, string Name, FileInput? Image, string SemanticImageName) : IRequest<Result<string>>,
        IHasName,
        IMayHasFileInput,
        IHasSemanticImageName;
}