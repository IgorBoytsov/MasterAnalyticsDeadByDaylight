namespace DBDAnalytics.Shared.Contracts.Responses.Offering
{
    public sealed record OfferingCategoryResponse(int Id, int OldId, string Name)
    {
        public static readonly OfferingCategoryResponse Empty = new(0, 0, string.Empty);
    }
}