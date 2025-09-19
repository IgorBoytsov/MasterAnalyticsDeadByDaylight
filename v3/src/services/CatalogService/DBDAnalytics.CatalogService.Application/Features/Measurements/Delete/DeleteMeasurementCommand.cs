using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.Delete
{
    public sealed record DeleteMeasurementCommand(Guid Id) : IRequest<Result>,
        IHasGuidId;
}