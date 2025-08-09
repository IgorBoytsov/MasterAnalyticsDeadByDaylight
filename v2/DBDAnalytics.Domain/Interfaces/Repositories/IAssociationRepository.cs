using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IAssociationRepository
    {
        Task<int> CreateAsync(string playerAssociationName, string playerAssociationDescription);
        Task<int> DeleteAsync(int idPlayerAssociation);
        bool Exist(int idPlayerAssociation);
        bool Exist(string playerAssociationName);
        Task<bool> ExistAsync(int idPlayerAssociation);
        Task<bool> ExistAsync(string playerAssociationName);
        IEnumerable<PlayerAssociationDomain> GetAll();
        Task<IEnumerable<PlayerAssociationDomain>> GetAllAsync();
        Task<PlayerAssociationDomain?> GetAsync(int idPlayerAssociation);
        Task<int> UpdateAsync(int idPlayerAssociation, string playerAssociationName, string playerAssociationDescription);
    }
}