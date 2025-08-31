namespace DBDAnalytics.Shared.Contracts.Responses.Survivor
{
    public sealed record SurvivorPerkResponse(string Id, int OldId, string Name, string? ImageKey, string SurvivorId, int? CategoryId);
}