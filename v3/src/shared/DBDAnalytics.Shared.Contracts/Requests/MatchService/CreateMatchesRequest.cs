namespace DBDAnalytics.Shared.Contracts.Requests.MatchService
{
    public sealed record CreateMatchesRequest(
    string UserId,
    int GameModeId, int GameEventId,
    string MapId, int WhoPlacedMapId, int WhoPlacedMapWinId,
    int PatchId,
    int CountKills, int CountHooks, int CountRecentGenerators,
    string StartedAt, string Duration,
    List<CreateMatchKillerDataRequest> Killers, List<CreateMatchSurvivorDataRequest> Survivors);

    public sealed record CreateMatchKillerDataRequest(
        string KillerId,
        string OfferingId,
        int AssociationId, int PlatformId,
        int Prestige, int Score, int Experience, int NumberBloodDrops,
        bool IsBot, bool IsAnonymousMode,
        List<string> Perks, List<string> Addons);

    public sealed record CreateMatchSurvivorDataRequest(
        string SurvivorId,
        string OfferingId,
        int AssociationId, int PlatformId, int TypeDeathId,
        int Prestige, int Score, int Experience, int NumberBloodDrops,
        bool IsBot, bool IsAnonymousMode,
        List<string> Perks, List<CreateMatchItemDataRequest> Items);

    public sealed record CreateMatchItemDataRequest(
        string ItemId,
        List<string> AddonIds);
}