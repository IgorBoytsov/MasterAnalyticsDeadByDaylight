namespace DBDAnalytics.Shared.Contracts.Responses.Survivor
{
    public sealed record SurvivorSoloResponse(string Id, int OldId, string Name, string? ImageKey);
}