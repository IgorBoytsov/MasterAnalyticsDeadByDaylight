using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IKillerPerkRepository
    {
        Task<int> CreateAsync(int idKiller, string PerkName, byte[]? perkImage, int? idCategory, string? perkDescription);
        Task<int> DeleteAsync(int idPerk);
        bool Exist(int idPerk);
        Task<bool> ExistAsync(int idPerk);
        IEnumerable<KillerPerkDomain> GetAll();
        Task<IEnumerable<KillerPerkDomain>> GetAllAsync();
        Task<KillerPerkDomain?> GetAsync(int idPerk);
        Task<int> UpdateAsync(int idPerk, int idKiller, string PerkName, byte[]? perkImage, int? idCategory, string? perkDescription);
    }
}