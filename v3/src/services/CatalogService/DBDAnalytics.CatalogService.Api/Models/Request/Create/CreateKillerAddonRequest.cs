namespace DBDAnalytics.CatalogService.Api.Models.Request.Create
{
    public sealed record CreateKillerAddonRequest(List<AddAddonToKillerRequestData> Addons);

    public sealed record AddAddonToKillerRequestData(int OldId, string Name, IFormFile? Image, string SemanticImageName);
}