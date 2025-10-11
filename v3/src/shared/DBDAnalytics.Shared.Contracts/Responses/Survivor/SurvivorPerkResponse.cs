namespace DBDAnalytics.Shared.Contracts.Responses.Survivor
{
    public sealed record SurvivorPerkResponse(string Id, int OldId, string Name, string? ImageKey, string SurvivorId, int? CategoryId)
    {
        public static readonly SurvivorPerkResponse Empty = new(string.Empty, 0, string.Empty, string.Empty, string.Empty, null);
    }
}