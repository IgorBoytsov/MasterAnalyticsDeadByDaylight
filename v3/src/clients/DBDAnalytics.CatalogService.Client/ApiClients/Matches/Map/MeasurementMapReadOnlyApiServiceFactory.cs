namespace DBDAnalytics.CatalogService.Client.ApiClients.Matches.Map
{
    public class MeasurementMapReadOnlyApiServiceFactory(IHttpClientFactory httpClientFactory) : IMeasurementMapReadOnlyApiServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _clientName = "CatalogApiClient";

        public IMeasurementMapReadOnlyApiService Create(string measurementsId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            return new MeasurementMapApiService(httpClient, measurementsId);
        }
    }
}