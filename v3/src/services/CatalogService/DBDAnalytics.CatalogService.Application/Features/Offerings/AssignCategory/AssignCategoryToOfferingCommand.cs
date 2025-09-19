using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.AssignCategory
{
    public sealed record AssignCategoryToOfferingCommand(Guid OfferingId, int CategoryId) : IRequest<Result>,
        IMustHasCategoryId;
}