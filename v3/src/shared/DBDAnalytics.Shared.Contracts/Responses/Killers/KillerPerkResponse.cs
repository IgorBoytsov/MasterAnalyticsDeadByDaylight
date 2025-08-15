namespace DBDAnalytics.Shared.Contracts.Responses.Killers
{
    public sealed record KillerPerkResponse(string Id, int OldId, string Name, string? ImageKey, string KillerId);
}