using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.Maps.Delete
{
    public sealed record DeleteMapCommand(Guid MeasurementId, Guid MapId) : IRequest<Result>;
}