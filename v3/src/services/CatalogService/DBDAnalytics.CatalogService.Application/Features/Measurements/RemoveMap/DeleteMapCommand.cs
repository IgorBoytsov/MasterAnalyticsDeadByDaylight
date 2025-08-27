using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.RemoveMap
{
    public sealed record DeleteMapCommand(Guid MeasurementId, Guid MapId) : IRequest<Result>;
}