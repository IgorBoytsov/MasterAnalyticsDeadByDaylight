using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.AssignCategory
{
    public sealed record AssignCategoryToOfferingCommand(Guid OfferingId, int CategoryId) : IRequest<Result>,
        IMustHasCategoryId;
}