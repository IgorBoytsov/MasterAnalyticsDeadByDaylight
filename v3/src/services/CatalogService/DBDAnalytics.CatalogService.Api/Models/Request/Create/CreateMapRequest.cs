namespace DBDAnalytics.CatalogService.Api.Models.Request.Create
{
    public sealed record CreateMapRequest(List<CreateMapRequestData> Maps);

    public sealed record CreateMapRequestData(int OldId, string Name, IFormFile? Image, string SemanticImageName);
}
