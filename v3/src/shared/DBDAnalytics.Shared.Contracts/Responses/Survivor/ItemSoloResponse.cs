namespace DBDAnalytics.Shared.Contracts.Responses.Survivor
{
    public sealed record ItemSoloResponse(string Id, int OldId, string Name, string? ImageKey)
    {
        public static readonly ItemSoloResponse Empty = new(string.Empty, 0, string.Empty, string.Empty);
    }
}