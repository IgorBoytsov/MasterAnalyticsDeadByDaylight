namespace DBDAnalytics.Shared.Contracts.Responses.Killers
{
    public sealed record KillerResponse(
        string Id, 
        string Name, 
        string? KillerImageKey, string? AbilityImageKey, 
        List<KillerAddonResponse> Addons, List<KillerPerkResponse> Perks);
}