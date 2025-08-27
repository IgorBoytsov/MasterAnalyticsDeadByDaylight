using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.SurvivorPerkCategories.Delete
{
    public sealed record DeleteSurvivorPerkCategoryCommand(int Id) : IRequest<Result>,
        IHasIntId;
}