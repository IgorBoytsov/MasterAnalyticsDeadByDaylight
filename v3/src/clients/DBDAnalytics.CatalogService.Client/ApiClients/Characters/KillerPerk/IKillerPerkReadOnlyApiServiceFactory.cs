namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerPerk
{
    public interface IKillerPerkReadOnlyApiServiceFactory
    {
        IKillerPerkReadOnlyService Create(string killerId);
    }
}