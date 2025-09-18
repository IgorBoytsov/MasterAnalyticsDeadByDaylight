namespace DBDAnalytics.CatalogService.Client.Models
{
    public sealed record ClientUpdateKillerAddonRequest(string Id, string NewName, string? ImagePath, string SemanticName);
}