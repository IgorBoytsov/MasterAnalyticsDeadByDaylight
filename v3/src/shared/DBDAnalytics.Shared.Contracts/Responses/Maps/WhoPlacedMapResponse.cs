namespace DBDAnalytics.Shared.Contracts.Responses.Maps
{
    public sealed record WhoPlacedMapResponse(int Id, int OldId, string Name)
    {
        public static readonly WhoPlacedMapResponse Empty = new(0, 0, string.Empty);
    }
}