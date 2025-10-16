using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Perks.GetAllSurvivor
{
    public sealed record GetAllSurvivorPerksQuery : IRequest<List<SurvivorPerkResponse>>;
}