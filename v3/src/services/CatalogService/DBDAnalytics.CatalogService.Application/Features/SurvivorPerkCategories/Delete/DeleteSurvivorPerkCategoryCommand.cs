using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.SurvivorPerkCategories.Delete
{
    public sealed record DeleteSurvivorPerkCategoryCommand(int Id) : IRequest<Result>,
        IHasIntId;
}