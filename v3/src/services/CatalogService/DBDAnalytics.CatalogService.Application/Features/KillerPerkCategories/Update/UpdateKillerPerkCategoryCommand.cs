using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.KillerPerkCategories.Update
{
    public sealed record UpdateKillerPerkCategoryCommand(int KillerPerkCategoryId, string Name) : IRequest<Result>,
        IHasName;
}