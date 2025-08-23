namespace DBDAnalytics.Shared.Contracts.Responses.Survivor
{
    public sealed record SurvivorResponse(string Id, int OldId, string Name, string? ImageKey, List<SurvivorPerkResponse> SurvivorPerks);
}
