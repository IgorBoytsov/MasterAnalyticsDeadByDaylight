using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface ISurvivorRepository
    {
        Task<int> CreateAsync(string survivorName, byte[]? survivorImage, string? survivorDescription);
        Task<int> DeleteAsync(int idSurvivor);
        bool Exist(int idSurvivor);
        bool Exist(string survivorName);
        Task<bool> ExistAsync(int idSurvivor);
        Task<bool> ExistAsync(string survivorName);
        IEnumerable<SurvivorDomain> GetAll();
        Task<IEnumerable<SurvivorDomain>> GetAllAsync();
        Task<SurvivorDomain?> GetAsync(int idSurvivor);
        Task<IEnumerable<SurvivorWithPerksDomain>> GetSurvivorsWithPerksAsync();
        Task<SurvivorWithPerksDomain?> GetSurvivorWithPerksAsync(int idSurvivor);
        Task<int> UpdateAsync(int idSurvivor, string survivorName, byte[]? survivorImage, string? survivorDescription);
    }
}