using DBDAnalytics.Shared.Contracts.Responses.MatchService;
using MediatR;

namespace DBDAnalytics.MatchService.Application.Features.Matches.GetAllForSurvivorSide
{
    public sealed record GetAllMatchesForSurvivorSideQuery(Guid UserId) : IRequest<List<MatchSurvivorItemResponse>>;
}