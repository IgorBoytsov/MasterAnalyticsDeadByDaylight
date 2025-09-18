namespace DBDAnalytics.CatalogService.Client.ApiClients.Loadout.ItemAddon
{
    internal sealed class ItemAddonApiServiceFactory(IHttpClientFactory httpClientFactory) : IItemAddonApiServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _clientName = "CatalogApiClient";

        public IItemAddonService Create(string itemId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            return new ItemAddonApiService(httpClient, itemId);
        }
    }
}