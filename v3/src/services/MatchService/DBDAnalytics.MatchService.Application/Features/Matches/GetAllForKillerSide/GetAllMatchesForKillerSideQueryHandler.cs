using DBDAnalytics.MatchService.Application.Abstractions.Contexts;
using DBDAnalytics.Shared.Contracts.Responses.MatchService;
using DBDAnalytics.Shared.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.MatchService.Application.Features.Matches.GetAllForKillerSide
{
    public sealed class GetAllMatchesForKillerSideQueryHandler(IReadDbContext readContext) : IRequestHandler<GetAllMatchesForKillerSideQuery, List<MatchKillerItemResponse>>
    {
        private readonly IReadDbContext _readContext = readContext;

        public async Task<List<MatchKillerItemResponse>> Handle(GetAllMatchesForKillerSideQuery request, CancellationToken cancellationToken)
            => await _readContext.MatchListItems
                .Where(m => m.UserId == request.UserId)
                    .Where(m => m.PlayerAssociationId == SmartPlayerAssociations.Me.Id)
                        .Where(m => m.PlayerRole == "Killer")
                            .OrderByDescending(m => m.StartedAt)
                                .Select(m => new MatchKillerItemResponse(
                                        m.MatchId.ToString(), 
                                        m.PlayerCharacterId.ToString(), 
                                        m.MapId.ToString(), 
                                        m.StartedAt.ToString(), 
                                        m.Duration.ToString(), 
                                        m.CountKills, 
                                        m.CountHooks, 
                                        m.CountRecentGenerators))
                                    .ToListAsync(cancellationToken);
    }
}