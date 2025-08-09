using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IItemRepository
    {
        Task<int> CreateAsync(string itemName, byte[] itemImage, string itemDescription);
        Task<int> DeleteAsync(int idItem);
        bool Exist(int idItem);
        bool Exist(string itemName);
        Task<bool> ExistAsync(int idItem);
        Task<bool> ExistAsync(string itemName);
        ItemDomain? Get(int idItem);
        Task<ItemDomain?> GetAsync(int idItem);
        Task<IEnumerable<ItemDomain>> GetAllAsync();
        IEnumerable<ItemDomain> GetAll();
        IEnumerable<ItemWithAddonsDomain> GetItemsWithAddons();
        Task<IEnumerable<ItemWithAddonsDomain>> GetItemsWithAddonsAsync();
        ItemWithAddonsDomain GetItemWithAddons(int idItem);
        Task<ItemWithAddonsDomain> GetItemWithAddonsAsync(int idItem);
        Task<int> UpdateAsync(int idItem, string itemName, byte[]? itemImage, string? itemDescription);
    }
}