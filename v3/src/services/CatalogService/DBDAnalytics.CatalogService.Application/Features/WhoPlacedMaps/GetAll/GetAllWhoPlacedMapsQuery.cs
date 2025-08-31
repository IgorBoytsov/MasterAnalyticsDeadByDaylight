using DBDAnalytics.Shared.Contracts.Responses.Maps;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.WhoPlacedMaps.GetAll
{
    public sealed record GetAllWhoPlacedMapsQuery() : IRequest<List<WhoPlacedMapResponse>>;
}