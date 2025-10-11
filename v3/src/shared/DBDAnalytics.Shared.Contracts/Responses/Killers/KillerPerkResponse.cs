namespace DBDAnalytics.Shared.Contracts.Responses.Killers
{
    public sealed record KillerPerkResponse(string Id, int OldId, string Name, string? ImageKey, string KillerId)
    {
        public static readonly KillerPerkResponse Empty = new(string.Empty, 0, string.Empty, string.Empty, string.Empty);
    }
}