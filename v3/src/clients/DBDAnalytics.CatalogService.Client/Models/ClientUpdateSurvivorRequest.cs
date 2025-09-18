namespace DBDAnalytics.CatalogService.Client.Models
{
    public sealed record ClientUpdateSurvivorRequest(string Id, string NewName, string ImagePath, string SemanticName);
}