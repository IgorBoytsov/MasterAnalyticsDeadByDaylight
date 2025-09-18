namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerPerk
{
    public class KillerPerkApiServiceFactory(IHttpClientFactory httpClientFactory) : IKillerPerkApiServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _clientName = "CatalogApiClient";

        public IKillerPerkService Create(string killerId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            return new KillerPerkApiService(httpClient, killerId);
        }
    }
}