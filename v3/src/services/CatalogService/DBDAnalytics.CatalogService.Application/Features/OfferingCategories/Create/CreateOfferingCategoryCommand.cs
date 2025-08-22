using DBDAnalytics.Shared.Contracts.Responses.Offering;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.OfferingCategories.Create
{
    public sealed record CreateOfferingCategoryCommand(int OldId, string Name) : IRequest<Result<OfferingCategoryResponse>>,
        IHasName;
}