namespace DBDAnalytics.Shared.Contracts.Responses.Match
{
    public sealed record GameModeResponse(int Id, int OldId, string Name)
    {
        public static readonly GameModeResponse Empty = new(0, 0, string.Empty);
    }
}