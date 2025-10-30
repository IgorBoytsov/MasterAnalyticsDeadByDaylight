namespace DBDAnalytics.Shared.Contracts.Responses.MatchService
{
    public sealed record MatchKillerItemResponse(
        string MatchId,
        string KillerId,
        string MapId,
        string StartedAt,
        string Duration,
        int CountKills,
        int CountHooks,
        int CountRecentGenerators);
}