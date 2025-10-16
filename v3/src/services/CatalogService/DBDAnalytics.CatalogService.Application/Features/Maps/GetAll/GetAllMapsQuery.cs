using DBDAnalytics.Shared.Contracts.Responses.Maps;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Maps.GetAll
{
    public sealed record GetAllMapsQuery() : IRequest<List<MapResponse>>;
}