using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Perks.AssignCategory
{
    public sealed record AssignCategoryToPerkCommand(Guid KillerId, Guid PerkId, int CategoryId) : IRequest<Result>,
        IHasKillerId,
        IHasPerkId,
        IMustHasCategoryId;
}