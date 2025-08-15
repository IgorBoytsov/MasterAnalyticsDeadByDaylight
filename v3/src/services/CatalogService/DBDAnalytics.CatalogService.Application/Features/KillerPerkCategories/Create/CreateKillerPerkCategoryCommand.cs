using DBDAnalytics.Shared.Contracts.Responses.Killers;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.KillerPerkCategories.Create
{
    public sealed record CreateKillerPerkCategoryCommand(int OldId, string Name) : 
        IRequest<Result<KillerPerkCategoryResponse>>,
        IHasName;
}