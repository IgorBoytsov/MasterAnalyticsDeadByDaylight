using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.SurvivorPerkCategories.Update
{
    public sealed record UpdateSurvivorPerkCategoryCommand(int SurvivorPerkCategoryId, string Name) : IRequest<Result>,
        IHasName;
}