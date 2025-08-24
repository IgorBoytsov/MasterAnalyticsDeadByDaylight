namespace DBDAnalytics.Shared.Contracts.Responses.Survivor
{
    public sealed record ItemAddonResponse(string Id, int OldId, string ItemId, string Name, string ImageKey, int? RarityId);
}