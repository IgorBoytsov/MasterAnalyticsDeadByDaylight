namespace DBDAnalytics.CatalogService.Client.Models
{
    public sealed record ClientUpdateSurvivorPerkRequest(string Id, string NewName, string ImagePath, string SemanticName);
}