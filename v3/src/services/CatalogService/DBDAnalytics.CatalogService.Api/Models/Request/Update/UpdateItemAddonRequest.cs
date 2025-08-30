namespace DBDAnalytics.CatalogService.Api.Models.Request.Update
{
    public sealed record UpdateItemAddonRequest(string NewName, IFormFile? Image, string SematicName);
}