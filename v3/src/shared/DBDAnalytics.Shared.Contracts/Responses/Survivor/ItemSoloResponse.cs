namespace DBDAnalytics.Shared.Contracts.Responses.Survivor
{
    public sealed record ItemSoloResponse(string Id, int OldId, string Name, string? ImageKey);
}