using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.OfferingCategories.Delete
{
    public sealed record DeleteOfferingCategoryCommand(int Id) : IRequest<Result>,
        IHasIntId;
}