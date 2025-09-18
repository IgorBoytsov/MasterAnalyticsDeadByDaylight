namespace DBDAnalytics.CatalogService.Client.ApiClients.Matches.Map
{
    internal class MapApiServiceFactory(IHttpClientFactory httpClientFactory) : IMapApiServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _clientName = "CatalogApiClient";

        public IMapApiService Create(string measurementsId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            return new MapApiService(httpClient, measurementsId);
        }
    }
}