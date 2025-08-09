using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface ISurvivorPerkRepository
    {
        Task<int> CreateAsync(int idSurvivor, string PerkName, byte[]? perkImage, int? idCategory, string? perkDescription);
        Task<int> DeleteAsync(int idPerk);
        bool Exist(int idPerk);
        Task<bool> ExistAsync(int idPerk);
        IEnumerable<SurvivorPerkDomain> GetAll();
        Task<IEnumerable<SurvivorPerkDomain>> GetAllAsync();
        Task<SurvivorPerkDomain?> GetAsync(int idPerk);
        Task<int> UpdateAsync(int idPerk, int idSurvivor, string PerkName, byte[]? perkImage, int? idCategory, string? perkDescription);
    }
}