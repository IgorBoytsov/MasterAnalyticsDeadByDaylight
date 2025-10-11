namespace DBDAnalytics.Shared.Contracts.Responses.CharacterInfo
{
    public sealed record PlatformResponse(int Id, int OldId, string Name)
    {
        public static readonly PlatformResponse Empty = new(0,0, string.Empty);
    }
}