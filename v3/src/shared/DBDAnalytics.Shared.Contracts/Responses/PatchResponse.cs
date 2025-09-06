namespace DBDAnalytics.Shared.Contracts.Responses
{
    public sealed record PatchResponse(int Id, int OldId, string Name, string Date)
    {
        public static readonly PatchResponse Empty = new(0, 0, string.Empty, new DateTime(2016, 6, 14, 0, 0, 0, DateTimeKind.Utc).ToString());
    }
}