namespace DBDAnalytics.CatalogService.Client.ApiClients.Loadout.ItemAddon
{
    public interface IItemAddonReadOnlyApiServiceFactory
    {
        IItemAddonReadOnlyService Create(string itemId);
    }
}