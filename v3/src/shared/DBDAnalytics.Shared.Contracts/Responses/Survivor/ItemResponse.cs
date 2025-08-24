namespace DBDAnalytics.Shared.Contracts.Responses.Survivor
{
    public sealed record ItemResponse(string Id, int OldId, string Name, string? ImageKey, List<ItemAddonResponse> ItemAddons);
}