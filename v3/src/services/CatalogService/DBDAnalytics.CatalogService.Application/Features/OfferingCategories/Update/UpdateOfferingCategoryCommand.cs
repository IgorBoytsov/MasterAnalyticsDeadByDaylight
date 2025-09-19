using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.OfferingCategories.Update
{
    public sealed record UpdateOfferingCategoryCommand(int OfferingCategoryId, string Name) : IRequest<Result>,
        IHasName;
}