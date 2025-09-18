namespace DBDAnalytics.CatalogService.Client.ApiClients.Loadout.ItemAddon
{
    public interface IItemAddonApiServiceFactory
    {
        IItemAddonService Create(string killerId);
    }
}