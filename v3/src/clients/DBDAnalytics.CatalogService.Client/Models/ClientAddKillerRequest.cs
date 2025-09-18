namespace DBDAnalytics.CatalogService.Client.Models
{
    public sealed record ClientAddKillerRequest(
        int OldId, string Name, 
        string? KillerImagePath, string? KillerAbilityImagePath, 
        List<ClientKillerAddonsData> Addons, List<ClientKillerPerksData> Perks);

    public sealed record ClientKillerAddonsData(int OldId, string Name, string? ImagePath);
    public sealed record ClientKillerPerksData(int OldId, string Name, string? ImagePath);
}