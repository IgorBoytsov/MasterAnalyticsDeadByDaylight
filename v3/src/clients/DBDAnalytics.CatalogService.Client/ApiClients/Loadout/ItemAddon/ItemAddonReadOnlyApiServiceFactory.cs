namespace DBDAnalytics.CatalogService.Client.ApiClients.Loadout.ItemAddon
{
    public sealed class ItemAddonReadOnlyApiServiceFactory(IHttpClientFactory httpClientFactory) : IItemAddonReadOnlyApiServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _clientName = "CatalogApiClient";

        public IItemAddonReadOnlyService Create(string itemId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            return new ItemAddonApiService(httpClient, itemId);
        }
    }
}