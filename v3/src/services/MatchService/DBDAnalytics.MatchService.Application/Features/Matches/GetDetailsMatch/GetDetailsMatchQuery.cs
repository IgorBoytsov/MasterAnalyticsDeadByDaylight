using DBDAnalytics.Shared.Contracts.Responses.MatchService;
using MediatR;

namespace DBDAnalytics.MatchService.Application.Features.Matches.GetDetailsMatch
{
    public sealed record GetDetailsMatchQuery(Guid MatchId) : IRequest<MatchDetailResponse>;
}