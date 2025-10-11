namespace DBDAnalytics.Shared.Contracts.Responses.Survivor
{
    public sealed record SurvivorSoloResponse(string Id, int OldId, string Name, string? ImageKey)
    {
        public static readonly SurvivorSoloResponse Empty = new(string.Empty, 0, string.Empty, string.Empty);
    }
}