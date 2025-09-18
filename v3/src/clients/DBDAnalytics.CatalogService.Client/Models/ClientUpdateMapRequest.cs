namespace DBDAnalytics.CatalogService.Client.Models
{
    public sealed record ClientUpdateMapRequest(string Id, string NewName, string? ImagePath, string SemanticName);
}