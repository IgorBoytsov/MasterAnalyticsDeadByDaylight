namespace DBDAnalytics.CatalogService.Client.Models
{
    public sealed record ClientAddSurvivorRequest(int OldId, string Name, string? ImagePath, List<ClientSurvivorPerkData> Perks);

    public sealed record ClientSurvivorPerkData(int OldId, string Name, string? ImagePath, int? CategoryId);
}
