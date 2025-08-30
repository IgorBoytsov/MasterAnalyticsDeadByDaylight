namespace DBDAnalytics.CatalogService.Api.Models.Request.Update
{
    public sealed record UpdateKillerAddonRequest(string NewName, IFormFile? Image, string SematicName);
}