namespace DBDAnalytics.CatalogService.Client.Models
{
    public sealed record ClientUpdateKillerPerkRequest(string Id, string NewName, string? ImagePath, string SemanticName);
}