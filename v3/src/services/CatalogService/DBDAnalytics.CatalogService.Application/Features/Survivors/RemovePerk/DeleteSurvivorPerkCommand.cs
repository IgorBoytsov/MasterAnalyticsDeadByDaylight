using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.RemovePerk
{
    public sealed record DeleteSurvivorPerkCommand(Guid SurvivorId, Guid PerkId) : IRequest<Result>;
}