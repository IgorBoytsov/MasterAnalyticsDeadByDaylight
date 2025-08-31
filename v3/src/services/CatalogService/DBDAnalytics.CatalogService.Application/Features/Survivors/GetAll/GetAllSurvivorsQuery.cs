using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.GetAll
{
    public sealed record GetAllSurvivorsQuery() : IRequest<List<SurvivorSoloResponse>>;
}