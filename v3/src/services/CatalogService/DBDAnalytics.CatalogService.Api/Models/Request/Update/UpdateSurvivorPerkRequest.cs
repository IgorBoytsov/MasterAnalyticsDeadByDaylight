namespace DBDAnalytics.CatalogService.Api.Models.Request.Update
{
    public sealed record UpdateSurvivorPerkRequest(string NewName, IFormFile? Image, string SemanticName);
}