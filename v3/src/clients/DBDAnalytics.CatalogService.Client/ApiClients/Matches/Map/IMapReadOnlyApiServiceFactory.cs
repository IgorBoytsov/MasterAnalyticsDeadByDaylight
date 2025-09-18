namespace DBDAnalytics.CatalogService.Client.ApiClients.Matches.Map
{
    public interface IMapReadOnlyApiServiceFactory
    {
        IMapReadOnlyApiService Create(string measurementsId);
    }
}