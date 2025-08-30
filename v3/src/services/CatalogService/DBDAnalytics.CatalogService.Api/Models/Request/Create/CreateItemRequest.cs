namespace DBDAnalytics.CatalogService.Api.Models.Request.Create
{
    public sealed record CreateItemRequest(int OldId, string Name, IFormFile? Image, string SemanticImageName, List<CreateItemAddonRequestData> Addons);

    public sealed record CreateItemAddonRequestData(int OldId, string Name, IFormFile? Image, string SemanticImageName, int? RarityId);
}