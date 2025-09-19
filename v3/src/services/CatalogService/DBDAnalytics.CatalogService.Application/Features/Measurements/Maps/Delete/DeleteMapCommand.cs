using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.Maps.Delete
{
    public sealed record DeleteMapCommand(Guid MeasurementId, Guid MapId) : IRequest<Result>;
}