namespace DBDAnalytics.Shared.Contracts.Responses.MatchService
{
    public sealed record MatchSurvivorItemResponse(
        string MatchId,
        string SurvivorId,
        string MapId,
        string StartedAt,
        string Duration,
        int CountKills,
        int CountHooks,
        int CountRecentGenerators,
        int? TypeDeath);
}