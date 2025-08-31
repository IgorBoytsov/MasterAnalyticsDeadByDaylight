using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.Perks.AssignCategory
{
    public sealed record AssignCategoryToPerkCommand(Guid SurvivorId, Guid PerkId, int CategoryId) : IRequest<Result>,
        IHasPerkId,
        IMustHasCategoryId;
}