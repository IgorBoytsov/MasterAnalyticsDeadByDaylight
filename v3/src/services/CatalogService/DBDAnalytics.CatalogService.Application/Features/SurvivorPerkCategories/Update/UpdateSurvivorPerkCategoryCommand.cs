using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.SurvivorPerkCategories.Update
{
    public sealed record UpdateSurvivorPerkCategoryCommand(int SurvivorPerkCategoryId, string Name) : IRequest<Result>,
        IHasName;
}