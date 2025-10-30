using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.MatchService.Application.Features.Matches.Create
{
    public sealed record CreateMatchCommand(
        Guid UserId,
        int GameModeId, int GameEventId, 
        Guid MapId, int WhoPlacedMapId, int WhoPlacedMapWinId,
        int PatchId,
        int CountKills, int CountHooks, int CountRecentGenerators,
        DateTime StartedAt, TimeSpan Duration,
        List<CreateMatchKillerData> Killers, List<CreateMatchSurvivorData> Survivors) : IRequest<Result>;

    public sealed record CreateMatchKillerData(
        Guid KillerId, 
        Guid OfferingId, 
        int AssociationId, int PlatformId, 
        int Prestige, int Score, int Experience, int NumberBloodDrops,
        bool IsBot, bool IsAnonymousMode,
        List<Guid> Perks, List<Guid> Addons);

    public sealed record CreateMatchSurvivorData(
        Guid SurvivorId,
        Guid OfferingId,
        int AssociationId, int PlatformId, int TypeDeathId,
        int Prestige, int Score, int Experience, int NumberBloodDrops,
        bool IsBot, bool IsAnonymousMode,
        List<Guid> Perks, List<CreateMatchItemData> Items);

    public sealed record CreateMatchItemData(
        Guid ItemId,
        List<Guid> AddonIds);
}