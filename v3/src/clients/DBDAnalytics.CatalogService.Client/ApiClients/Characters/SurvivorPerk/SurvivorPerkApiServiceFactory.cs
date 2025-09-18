namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.SurvivorPerk
{
    internal sealed class SurvivorPerkApiServiceFactory(IHttpClientFactory httpClientFactory) : ISurvivorPerkApiServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _clientName = "CatalogApiClient";

        public ISurvivorPerkService Create(string survivorId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            return new SurvivorPerkApiService(httpClient, survivorId);
        }
    }
}