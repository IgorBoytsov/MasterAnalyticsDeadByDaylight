namespace DBDAnalytics.Shared.Contracts.Responses.Survivor
{
    public sealed record SurvivorPerkCategoryResponse(int Id, int OldId, string Name)
    {
        public static readonly SurvivorPerkCategoryResponse Empty = new(0, 0, string.Empty);
    }
}