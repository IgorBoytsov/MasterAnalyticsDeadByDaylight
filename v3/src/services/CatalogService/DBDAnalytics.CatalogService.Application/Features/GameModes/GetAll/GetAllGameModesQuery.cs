using DBDAnalytics.Shared.Contracts.Responses.Match;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.GameModes.GetAll
{
    public sealed record GetAllGameModesQuery() : IRequest<List<GameModeResponse>>;
}