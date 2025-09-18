namespace DBDAnalytics.CatalogService.Client.ApiClients.Matches.Map
{
    public interface IMapApiServiceFactory
    {
        IMapApiService Create(string measurementsId);
    }
}