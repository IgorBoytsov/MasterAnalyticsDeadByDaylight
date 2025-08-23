using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.SurvivorPerkCategories.Create
{
    public sealed record CreateSurvivorPerkCategoryCommand(int OldId, string Name) : IRequest<Result<SurvivorPerkCategoryResponse>>,
        IHasName;
}