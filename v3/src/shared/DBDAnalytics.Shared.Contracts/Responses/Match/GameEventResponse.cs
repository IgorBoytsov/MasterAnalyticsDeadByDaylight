namespace DBDAnalytics.Shared.Contracts.Responses.Match
{
    public sealed record GameEventResponse(int Id, int OldId, string Name)
    {
        public static readonly GameEventResponse Empty = new(0, 0, string.Empty);
    }
}