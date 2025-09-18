namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.SurvivorPerk
{
    public sealed class SurvivorPerkReadOnlyApiServiceFactory(IHttpClientFactory httpClientFactory) : ISurvivorPerkReadOnlyApiServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _clientName = "CatalogApiClient";

        public ISurvivorPerkReadOnlyService Create(string survivorId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            return new SurvivorPerkApiService(httpClient, survivorId);
        }
    }
}