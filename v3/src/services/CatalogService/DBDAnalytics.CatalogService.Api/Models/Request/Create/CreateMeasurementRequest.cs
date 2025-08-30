namespace DBDAnalytics.CatalogService.Api.Models.Request.Create
{
    public sealed record MeasurementRequest(int OldId, string Name, List<MapRequestData> Maps);

    public sealed record MapRequestData(int OldId, string Name, IFormFile? Image, string SemanticImageName);
}