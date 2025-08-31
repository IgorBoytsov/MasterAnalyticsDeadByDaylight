using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.Perks.Delete
{
    public sealed record DeleteSurvivorPerkCommand(Guid SurvivorId, Guid PerkId) : IRequest<Result>;
}