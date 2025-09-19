using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.KillerPerkCategories.Delete
{
    public sealed record DeleteKillerPerkCategoryCommand(int Id) : IRequest<Result>,
        IHasIntId;
}