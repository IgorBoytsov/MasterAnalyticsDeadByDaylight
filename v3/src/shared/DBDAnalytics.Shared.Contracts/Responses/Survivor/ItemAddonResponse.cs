namespace DBDAnalytics.Shared.Contracts.Responses.Survivor
{
    public sealed record ItemAddonResponse(string Id, int OldId, string ItemId, string Name, string ImageKey, int? RarityId)
    {
        public static readonly ItemAddonResponse Empty = new(string.Empty, 0, string.Empty, string.Empty, string.Empty, null);
    }
}