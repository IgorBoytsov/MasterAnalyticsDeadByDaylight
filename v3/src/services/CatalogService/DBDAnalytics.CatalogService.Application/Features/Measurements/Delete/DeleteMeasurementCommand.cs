using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.Delete
{
    public sealed record DeleteMeasurementCommand(Guid Id) : IRequest<Result>,
        IHasGuidId;
}