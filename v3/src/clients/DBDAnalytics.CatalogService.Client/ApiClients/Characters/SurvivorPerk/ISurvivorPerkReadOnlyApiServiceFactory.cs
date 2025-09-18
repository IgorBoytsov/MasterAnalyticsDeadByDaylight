namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.SurvivorPerk
{
    public interface ISurvivorPerkReadOnlyApiServiceFactory
    {
        ISurvivorPerkReadOnlyService Create(string survivorId);
    }
}