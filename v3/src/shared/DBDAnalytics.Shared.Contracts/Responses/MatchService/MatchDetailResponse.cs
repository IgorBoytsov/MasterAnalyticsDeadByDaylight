namespace DBDAnalytics.Shared.Contracts.Responses.MatchService
{
    public sealed record MatchDetailResponse(
        Guid MatchId,
        DateTime StartedAt,
        TimeSpan Duration,
        Guid MapId,
        int GameModeId,
        int GameEventId,
        int PatchId,
        KillerDetailsResponse KillerDetails,
        List<SurvivorDetailsResponse> SurvivorDetails);

    public sealed record KillerDetailsResponse(Guid KillerId, int Prestige, int Score, int Experience, int NumberBloodDrops, int PlayerAssociationId, int PlatformId, bool IsBot, bool IsAnonymousMode, List<Guid> PerkIds, List<Guid> AddonIds);
    
    public sealed record SurvivorDetailsResponse(Guid SurvivorId, int Prestige, int Score, int Experience, int NumberBloodDrops, int PlayerAssociationId, int PlatformId, int TypeDeathId, bool IsBot, bool IsAnonymousMode, List<Guid> PerkIds, SurvivorItemResponse Item);
    public sealed record SurvivorItemResponse(Guid ItemId, List<Guid> AddonIds);
}