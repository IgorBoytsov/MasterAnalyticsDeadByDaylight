namespace DBDAnalytics.CatalogService.Client.Models
{
    public sealed record ClientUpdateItemAddonRequest(string Id, string NewName, string? ImagePath, string SematicName);
}