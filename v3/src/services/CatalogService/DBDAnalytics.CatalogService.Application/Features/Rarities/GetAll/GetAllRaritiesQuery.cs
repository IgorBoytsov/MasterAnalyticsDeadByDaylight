using DBDAnalytics.Shared.Contracts.Responses;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Rarities.GetAll
{
    public sealed record GetAllRaritiesQuery() : IRequest<List<RarityResponse>>;
}