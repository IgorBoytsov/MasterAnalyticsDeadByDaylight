using DBDAnalytics.Shared.Contracts.Responses.Offering;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.OfferingCategories.Create
{
    public sealed record CreateOfferingCategoryCommand(int OldId, string Name) : IRequest<Result<OfferingCategoryResponse>>,
        IHasName;
}