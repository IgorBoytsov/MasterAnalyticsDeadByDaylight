namespace DBDAnalytics.Shared.Contracts.Responses.Killers
{
    public sealed record KillerSoloResponse(string Id, int OldId, string Name, string? KillerImageKey, string? AbilityImageKey)
    {
        public static readonly KillerSoloResponse Empty = new(string.Empty, 0, string.Empty, string.Empty, string.Empty);
    }
}