namespace DBDAnalytics.Shared.Contracts.Responses.Killers
{
    public sealed record KillerAddonResponse(string Id, int OldId, string Name, string? ImageKey, string KillerId);
}
