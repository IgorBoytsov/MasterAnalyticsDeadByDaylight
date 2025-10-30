using DBDAnalytics.MatchService.Application.Abstractions.Contexts;
using DBDAnalytics.Shared.Contracts.Responses.MatchService;
using DBDAnalytics.Shared.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.MatchService.Application.Features.Matches.GetAllForSurvivorSide
{
    public sealed class GetAllMatchesForSurvivorSideQueryHandler(IReadDbContext readContext) : IRequestHandler<GetAllMatchesForSurvivorSideQuery, List<MatchSurvivorItemResponse>>
    {
        private readonly IReadDbContext _readContext = readContext;

        public async Task<List<MatchSurvivorItemResponse>> Handle(GetAllMatchesForSurvivorSideQuery request, CancellationToken cancellationToken)
            => await _readContext.MatchListItems
                .Where(m => m.UserId == request.UserId)
                    .Where(m => m.PlayerAssociationId == SmartPlayerAssociations.Me.Id)
                        .Where(m => m.PlayerRole == "Survivor")
                            .OrderByDescending(m => m.StartedAt)
                                .Select(m => new MatchSurvivorItemResponse(
                                        m.MatchId.ToString(), 
                                        m.PlayerCharacterId.ToString(), 
                                        m.MapId.ToString(), 
                                        m.StartedAt.ToString(), 
                                        m.Duration.ToString(), 
                                        m.CountKills, 
                                        m.CountHooks, 
                                        m.CountRecentGenerators, 
                                        m.TypeDeathId != null ? m.TypeDeathId.Value : 0))
                                    .ToListAsync(cancellationToken);
    }
}