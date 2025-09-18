namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerPerk
{
    public interface IKillerPerkApiServiceFactory
    {
        IKillerPerkService Create(string killerId);
    }
}