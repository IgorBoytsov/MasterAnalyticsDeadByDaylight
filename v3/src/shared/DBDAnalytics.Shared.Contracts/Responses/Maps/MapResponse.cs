namespace DBDAnalytics.Shared.Contracts.Responses.Maps
{
    public sealed record MapResponse(string Id, int OldId, string Name, string? ImageKey, string MeasurementId)
    {
        public static readonly MapResponse Empty = new(string.Empty, 0, string.Empty, string.Empty, string.Empty);
    }
}