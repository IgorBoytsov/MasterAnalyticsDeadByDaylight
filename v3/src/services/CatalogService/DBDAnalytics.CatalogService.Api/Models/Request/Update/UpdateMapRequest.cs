namespace DBDAnalytics.CatalogService.Api.Models.Request.Update
{
    public sealed record UpdateMapRequest(string NewName, IFormFile? Image, string SemanticName);
}