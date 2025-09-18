namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerPerk
{
    public sealed class KillerPerkReadOnlyApiServiceFactory(IHttpClientFactory httpClientFactory) : IKillerPerkReadOnlyApiServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _clientName = "CatalogApiClient";

        public IKillerPerkReadOnlyService Create(string killerId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            return new KillerPerkApiService(httpClient, killerId);
        }
    }
}