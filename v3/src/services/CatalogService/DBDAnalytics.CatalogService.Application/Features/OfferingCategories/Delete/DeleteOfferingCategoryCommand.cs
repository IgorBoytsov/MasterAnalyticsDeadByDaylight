using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.OfferingCategories.Delete
{
    public sealed record DeleteOfferingCategoryCommand(int Id) : IRequest<Result>,
        IHasIntId;
}