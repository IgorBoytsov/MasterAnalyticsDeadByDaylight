using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.TypeDeaths.GetAll
{
    public sealed record GetAllTypeDeathsQuery() : IRequest<List<TypeDeathResponse>>;
}