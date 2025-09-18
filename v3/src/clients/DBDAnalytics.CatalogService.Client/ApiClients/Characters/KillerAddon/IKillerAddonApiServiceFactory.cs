namespace DBDAnalytics.CatalogService.Client.ApiClients.Characters.KillerAddon
{
    public interface IKillerAddonApiServiceFactory
    {
        IKillerAddonService Create(string killerId);
    }
}