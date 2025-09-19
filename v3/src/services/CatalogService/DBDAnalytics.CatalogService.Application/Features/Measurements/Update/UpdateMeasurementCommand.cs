using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.Update
{
    public sealed record UpdateMeasurementCommand(Guid MeasurementId, string Name) : IRequest<Result>,
        IHasName;
}