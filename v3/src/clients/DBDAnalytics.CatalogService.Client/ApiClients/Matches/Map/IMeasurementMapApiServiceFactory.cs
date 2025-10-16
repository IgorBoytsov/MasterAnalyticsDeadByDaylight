namespace DBDAnalytics.CatalogService.Client.ApiClients.Matches.Map
{
    public interface IMeasurementMapApiServiceFactory
    {
        IMeasurementMapApiService Create(string measurementsId);
    }
}