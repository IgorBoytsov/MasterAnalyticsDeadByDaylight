namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerAddon
{
    public interface IKillerAddonReadOnlyApiServiceFactory
    {
        IKillerAddonReadOnlyService Create(string killerId);
    }
}