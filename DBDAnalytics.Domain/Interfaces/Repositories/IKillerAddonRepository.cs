using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IKillerAddonRepository
    {
        Task<int> CreateAsync(int idKiller, int? idRarity, string addonName, byte[]? addonImage, string? addonDescription);
        Task<int> DeleteAsync(int idKillerAddon);
        bool Exist(int idKillerAddon);
        Task<bool> ExistAsync(int idKillerAddon);
        KillerAddonDomain? Get(int idKillerAddon);
        IEnumerable<KillerAddonDomain> GetAll();
        Task<IEnumerable<KillerAddonDomain>> GetAllAsync();
        Task<List<KillerAddonDomain>> GetAllByIdKiller(int idKiller);
        Task<KillerAddonDomain?> GetAsync(int idKillerAddon);
        Task<int> UpdateAsync(int idAddon, int idKiller, int? idRarity, string addonName, byte[]? addonImage, string? addonDescription);
    }
}