namespace DBDAnalytics.Shared.Contracts.Responses.Killers
{
    public sealed record KillerPerkCategoryResponse(int Id, int OldId, string Name)
    {
        public static readonly KillerPerkCategoryResponse Empty = new(0, 0, string.Empty);
    }
}