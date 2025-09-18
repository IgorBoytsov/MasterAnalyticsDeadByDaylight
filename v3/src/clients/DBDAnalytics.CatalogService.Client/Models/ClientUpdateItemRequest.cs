namespace DBDAnalytics.CatalogService.Client.Models
{
    public sealed record ClientUpdateItemRequest(string Id, string NewName, string ImagePath, string SemanticName);
}