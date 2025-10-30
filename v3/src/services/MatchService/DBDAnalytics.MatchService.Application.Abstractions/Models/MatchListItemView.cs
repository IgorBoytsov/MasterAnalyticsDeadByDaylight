namespace DBDAnalytics.MatchService.Application.Abstractions.Models
{
    public sealed record MatchListItemView(
        Guid MatchId,
        Guid UserId,
        int PlayerAssociationId,
        Guid PlayerCharacterId,
        string PlayerRole,
        int? TypeDeathId,
        DateTime StartedAt,
        TimeSpan Duration,
        Guid MapId,
        int CountKills,
        int CountHooks,
        int CountRecentGenerators);
}