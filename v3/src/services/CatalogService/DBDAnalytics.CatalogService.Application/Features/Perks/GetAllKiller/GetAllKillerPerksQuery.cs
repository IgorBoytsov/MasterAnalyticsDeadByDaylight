using DBDAnalytics.Shared.Contracts.Responses.Killers;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Perks.GetAllKiller
{
    public sealed record GetAllKillerPerksQuery : IRequest<List<KillerPerkResponse>>;
}