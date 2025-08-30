namespace DBDAnalytics.CatalogService.Api.Models.Request.Create
{
    public sealed record CreateItemAddonRequest(List<AddPerkToItemRequestData> Addons);

    public sealed record AddPerkToItemRequestData(int OldId, string Name, IFormFile? Image, string SemanticImageName, int? RarityId);
}