namespace DBDAnalytics.CatalogService.Client.ApiClients.Matches.Map
{
    internal class MeasurementMapApiServiceFactory(IHttpClientFactory httpClientFactory) : IMeasurementMapApiServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _clientName = "CatalogApiClient";

        public IMeasurementMapApiService Create(string measurementsId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            return new MeasurementMapApiService(httpClient, measurementsId);
        }
    }
}