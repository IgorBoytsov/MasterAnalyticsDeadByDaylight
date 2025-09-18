namespace DBDAnalytics.CatalogService.Client.Models
{
    public sealed record ClientAddMeasurementRequest(int OldId, string Name, List<ClientMapData> Maps);
    public sealed record ClientMapData(int OldId, string Name, string? ImagePath, string SemanticImageName);
}