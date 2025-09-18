namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerAddon
{
    public sealed class KillerAddonReadOnlyApiServiceFactory(IHttpClientFactory httpClientFactory) : IKillerAddonReadOnlyApiServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _clientName = "CatalogApiClient";

        public IKillerAddonReadOnlyService Create(string killerId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            return new KillerAddonApiService(httpClient, killerId);
        }
    }
}