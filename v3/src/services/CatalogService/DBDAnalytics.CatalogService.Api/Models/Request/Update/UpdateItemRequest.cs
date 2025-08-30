namespace DBDAnalytics.CatalogService.Api.Models.Request.Update
{
    public sealed record UpdateItemRequest(string NewName, IFormFile? NewImage, string SemanticName);
}