using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface ITypeDeathRepository
    {
        Task<int> CreateAsync(string typeDeathName, string typeDeathDescription);
        Task<int> DeleteAsync(int idTypeDeath);
        bool Exist(int idTypeDeath);
        bool Exist(string typeDeathName);
        Task<bool> ExistAsync(int idTypeDeath);
        Task<bool> ExistAsync(string typeDeathName);
        IEnumerable<TypeDeathDomain> GetAll();
        Task<IEnumerable<TypeDeathDomain>> GetAllAsync();
        Task<TypeDeathDomain?> GetAsync(int idTypeDeath);
        Task<int> UpdateAsync(int idTypeDeath, string typeDeathName, string typeDeathDescription);
    }
}