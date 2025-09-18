namespace DBDAnalytics.CatalogService.Client.Models
{
    public sealed record ClientAddItemRequest(int OldId, string Name, string? ImagePath, List<ClientItemAddonsData> Addons);

    public sealed record ClientItemAddonsData(int OldId, string Name, string? ImagePath, string SemanticImageName, int? RarityId);
}