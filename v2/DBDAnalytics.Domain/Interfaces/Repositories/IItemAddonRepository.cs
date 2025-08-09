using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IItemAddonRepository
    {
        Task<int> CreateAsync(int idItem, int? idRarity, string itemAddonName, byte[]? itemAddonImage, string? itemAddonDescription);
        Task<int> DeleteAsync(int idItemAddon);
        bool Exist(int idItemAddon);
        bool Exist(string itemAddonName);
        Task<bool> ExistAsync(int idItemAddon);
        Task<bool> ExistAsync(string itemAddonName);
        ItemAddonDomain? Get(int idItemAddon);
        IEnumerable<ItemAddonDomain> GetAll();
        Task<IEnumerable<ItemAddonDomain>> GetAllAsync();
        Task<ItemAddonDomain?> GetAsync(int idItemAddon);
        Task<int> UpdateAsync(int idItemAddon, int idItem, int? idRarity, string itemAddonName, byte[]? itemAddonImage, string? itemAddonDescription);
    }
}