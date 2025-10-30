using DBDAnalytics.MatchService.Application.Abstractions.Contexts;
using DBDAnalytics.Shared.Contracts.Responses.MatchService;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.MatchService.Application.Features.Matches.GetDetailsMatch
{
    public sealed class GetDetailsMatchQueryHandler(IReadDbContext readContext) : IRequestHandler<GetDetailsMatchQuery, MatchDetailResponse>
    {
        private readonly IReadDbContext _readContext = readContext;

        public async Task<MatchDetailResponse> Handle(GetDetailsMatchQuery request, CancellationToken cancellationToken)
        {
            var matchDetailsView = await _readContext.MatchDetails
                .FirstOrDefaultAsync(m => m.MatchId == request.MatchId, cancellationToken);

            if (matchDetailsView is null)
                return null;

            var response = new MatchDetailResponse(
                matchDetailsView.MatchId,
                matchDetailsView.StartedAt,
                matchDetailsView.Duration,
                matchDetailsView.MapId,
                matchDetailsView.GameModeId,
                matchDetailsView.GameEventId,
                matchDetailsView.PatchId,
                new KillerDetailsResponse(
                    matchDetailsView.KillerDetails.KillerId,
                    matchDetailsView.KillerDetails.Prestige,
                    matchDetailsView.KillerDetails.Score,
                    matchDetailsView.KillerDetails.Experience,
                    matchDetailsView.KillerDetails.NumberBloodDrops,
                    matchDetailsView.KillerDetails.PlayerAssociationId,
                    matchDetailsView.KillerDetails.PlatformId,
                    matchDetailsView.KillerDetails.IsBot,
                    matchDetailsView.KillerDetails.IsAnonymousMode,
                    matchDetailsView.KillerDetails.PerkIds,
                    matchDetailsView.KillerDetails.AddonIds
                ),
                [.. matchDetailsView.SurvivorDetails.Select(
                    s => new SurvivorDetailsResponse(
                    s.SurvivorId,
                    s.Prestige,
                    s.Score,
                    s.Experience,
                    s.NumberBloodDrops,
                    s.PlayerAssociationId,
                    s.PlatformId,
                    s.TypeDeathId,
                    s.IsBot,
                    s.IsAnonymousMode,
                    s.PerkIds,
                    new SurvivorItemResponse(
                        s.Item.ItemId,
                        s.Item.AddonIds
                    )
                ))]
            );

            return response;
        }
    }
}