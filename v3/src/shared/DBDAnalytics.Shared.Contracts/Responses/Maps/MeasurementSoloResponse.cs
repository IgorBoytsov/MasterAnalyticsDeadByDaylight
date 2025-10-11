namespace DBDAnalytics.Shared.Contracts.Responses.Maps
{
    public sealed record MeasurementSoloResponse(string Id, int OldId, string Name)
    {
        public static readonly MeasurementSoloResponse Empty = new(string.Empty, 0, string.Empty);
    }
}