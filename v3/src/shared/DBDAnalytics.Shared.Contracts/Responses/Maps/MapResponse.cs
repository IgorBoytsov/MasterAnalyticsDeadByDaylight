namespace DBDAnalytics.Shared.Contracts.Responses.Maps
{
    public sealed record MapResponse(string Id, int OldId, string Name, string? ImageKey, string MeasurementId);
}