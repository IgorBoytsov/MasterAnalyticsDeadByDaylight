using DBDAnalytics.Shared.Contracts.Responses.Offering;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.OfferingCategories.GetAll
{
    public sealed record GetAllOfferingCategoriesQuery() : IRequest<List<OfferingCategoryResponse>>;
}