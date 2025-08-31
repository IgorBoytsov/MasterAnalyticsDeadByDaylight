using DBDAnalytics.Shared.Contracts.Responses.Killers;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Perks.GetAll
{
    public sealed record GetAllKillerPerksQuery() : IRequest<List<KillerPerkResponse>>;
}