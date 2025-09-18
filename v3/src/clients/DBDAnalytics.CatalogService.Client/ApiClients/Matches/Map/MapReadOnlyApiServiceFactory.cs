namespace DBDAnalytics.CatalogService.Client.ApiClients.Matches.Map
{
    internal class MapReadOnlyApiServiceFactory(IHttpClientFactory httpClientFactory) : IMapReadOnlyApiServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _clientName = "CatalogApiClient";

        public IMapReadOnlyApiService Create(string measurementsId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            return new MapApiService(httpClient, measurementsId);
        }
    }
}