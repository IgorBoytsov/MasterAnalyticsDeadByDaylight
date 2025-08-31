using DBDAnalytics.Shared.Contracts.Responses.Maps;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.Maps.GetById
{
    public sealed record GetMapsByMeasurementIdQuery(Guid Id) : IRequest<List<MapResponse>>,
        IHasGuidId;
}