using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.OfferingCategories.Update
{
    public sealed record UpdateOfferingCategoryCommand(int OfferingCategoryId, string Name) : IRequest<Result>,
        IHasName;
}