namespace DBDAnalytics.Shared.Contracts.Responses.Offering
{
    public sealed record OfferingResponse(string Id, int OldId, string Name, string? ImageKey, int RoleId, int? RarityId, int? CategoryId)
    {
        public static readonly OfferingResponse Empty = new(string.Empty, 0, string.Empty, string.Empty, 0, null, null);
    }
}