namespace DBDAnalytics.CatalogService.Api.Models.Request.Update
{
    public sealed record UpdateKillerPerkRequest(string NewName, IFormFile? Image, string SemanticName);
}