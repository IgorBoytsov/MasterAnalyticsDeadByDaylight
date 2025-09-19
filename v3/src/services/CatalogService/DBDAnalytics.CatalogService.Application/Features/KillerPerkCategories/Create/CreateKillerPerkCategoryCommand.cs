using DBDAnalytics.Shared.Contracts.Responses.Killers;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.KillerPerkCategories.Create
{
    public sealed record CreateKillerPerkCategoryCommand(int OldId, string Name) : 
        IRequest<Result<KillerPerkCategoryResponse>>,
        IHasName;
}