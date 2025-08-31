using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.SurvivorPerkCategories.GetAll
{
    public sealed record GetAllSurvivorPerkCategoriesQuery() : IRequest<List<SurvivorPerkCategoryResponse>>;
}