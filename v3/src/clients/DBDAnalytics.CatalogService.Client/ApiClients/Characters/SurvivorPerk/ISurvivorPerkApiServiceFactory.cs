namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.SurvivorPerk
{
    public interface ISurvivorPerkApiServiceFactory
    {
        ISurvivorPerkService Create(string survivorId);
    }
}