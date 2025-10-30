using DBDAnalytics.Shared.Contracts.Responses.MatchService;
using MediatR;

namespace DBDAnalytics.MatchService.Application.Features.Matches.GetAllForKillerSide
{
    public sealed record GetAllMatchesForKillerSideQuery(Guid UserId) : IRequest<List<MatchKillerItemResponse>>;
}