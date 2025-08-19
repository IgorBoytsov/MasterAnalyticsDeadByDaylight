namespace DBDAnalytics.Shared.Contracts.Responses.Maps
{
    public sealed record MeasurementResponse(string Id, int OldId, string Name, List<MapResponse> Maps);
}