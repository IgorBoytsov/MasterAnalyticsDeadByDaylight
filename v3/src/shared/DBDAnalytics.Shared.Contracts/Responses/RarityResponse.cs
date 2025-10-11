namespace DBDAnalytics.Shared.Contracts.Responses
{
    public sealed record RarityResponse(int Id, int OldId, string Name)
    {
        public static readonly RarityResponse Empty = new(0, 0, string.Empty);
    }
}