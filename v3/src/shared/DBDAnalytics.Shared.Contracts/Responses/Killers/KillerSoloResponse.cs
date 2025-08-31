namespace DBDAnalytics.Shared.Contracts.Responses.Killers
{
    public sealed record KillerSoloResponse(string Id, string Name, string? KillerImageKey, string? AbilityImageKey);
}