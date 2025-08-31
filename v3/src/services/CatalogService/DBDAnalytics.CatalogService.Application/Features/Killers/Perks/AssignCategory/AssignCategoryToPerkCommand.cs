using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Perks.AssignCategory
{
    public sealed record AssignCategoryToPerkCommand(Guid KillerId, Guid PerkId, int CategoryId) : IRequest<Result>,
        IHasKillerId,
        IHasPerkId,
        IMustHasCategoryId;
}