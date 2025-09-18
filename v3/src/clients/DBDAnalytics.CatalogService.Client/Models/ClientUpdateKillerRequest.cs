namespace DBDAnalytics.CatalogService.Client.Models
{
    public sealed record ClientUpdateKillerRequest(string Id, string Name,
        string? ImageAbilityPath, string SemanticImageAbilityName,
        string? ImagePortraitPath, string SemanticImagePortraitName);
}