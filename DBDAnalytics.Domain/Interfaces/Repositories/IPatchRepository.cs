using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IPatchRepository
    {
        Task<int> CreateAsync(string patchNumber, DateOnly patchDateRelease);
        Task<int> DeleteAsync(int idPatch);
        bool Exist(int idPatch);
        bool Exist(string patchNumber);
        Task<bool> ExistAsync(int idPatch);
        Task<bool> ExistAsync(string patchNumber);
        IEnumerable<PatchDomain> GetAll();
        Task<IEnumerable<PatchDomain>> GetAllAsync();
        Task<PatchDomain?> GetAsync(int idPatch);
        Task<int> UpdateAsync(int idPatch, string patchNumber, DateOnly patchDateRelease);
    }
}