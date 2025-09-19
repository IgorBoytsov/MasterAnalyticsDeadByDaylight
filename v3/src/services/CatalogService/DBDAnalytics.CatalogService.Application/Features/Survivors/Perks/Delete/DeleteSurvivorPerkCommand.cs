using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.Perks.Delete
{
    public sealed record DeleteSurvivorPerkCommand(Guid SurvivorId, Guid PerkId) : IRequest<Result>;
}