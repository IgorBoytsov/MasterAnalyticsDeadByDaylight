namespace DBDAnalytics.CatalogService.Client.ApiClients.Matches.Map
{
    public interface IMeasurementMapReadOnlyApiServiceFactory
    {
        IMeasurementMapReadOnlyApiService Create(string measurementsId);
    }
}