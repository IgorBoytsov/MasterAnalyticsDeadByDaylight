using DBDAnalytics.Shared.Contracts.Responses.Killers;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.KillerPerkCategories.GetAll
{
    public sealed record GetAllKillerPerkCategoriesQuery() : IRequest<List<KillerPerkCategoryResponse>>;
}