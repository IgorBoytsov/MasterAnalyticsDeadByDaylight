using DBDAnalytics.Shared.Contracts.Responses.Maps;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.GetAll
{
    public sealed record GetAllMeasurementsQuery() : IRequest<List<MeasurementSoloResponse>>;
}