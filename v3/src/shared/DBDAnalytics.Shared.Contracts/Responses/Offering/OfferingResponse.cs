namespace DBDAnalytics.Shared.Contracts.Responses.Offering
{
    public sealed record OfferingResponse(string Id, int OldId, string Name, string? ImageKey, int RoleId, int? RarityId, int? CategoryId);
}