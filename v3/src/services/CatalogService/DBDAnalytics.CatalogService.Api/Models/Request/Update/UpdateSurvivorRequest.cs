namespace DBDAnalytics.CatalogService.Api.Models.Request.Update
{
    public sealed record UpdateSurvivorRequest(string NewName, IFormFile? Image, string SemanticName);
}