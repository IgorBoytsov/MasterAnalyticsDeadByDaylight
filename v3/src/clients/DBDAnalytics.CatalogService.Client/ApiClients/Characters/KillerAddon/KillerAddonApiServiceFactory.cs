namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerAddon
{
    public sealed class KillerAddonApiServiceFactory(IHttpClientFactory httpClientFactory) : IKillerAddonApiServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _clientName = "CatalogApiClient";

        public IKillerAddonService Create(string killerId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            return new KillerAddonApiService(httpClient, killerId);
        }
    }
}