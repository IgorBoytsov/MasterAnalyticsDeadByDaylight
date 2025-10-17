namespace DBDAnalytics.Shared.Contracts.Responses.Killers
{
    public sealed record KillerResponse(
        string Id,
        int OldId,
        string Name,
        string? KillerImageKey, string? AbilityImageKey,
        List<KillerAddonResponse> KillerAddons, List<KillerPerkResponse> KillerPerks);
}