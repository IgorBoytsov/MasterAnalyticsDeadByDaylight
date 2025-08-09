using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.AssociationCase
{
    public interface IGetAssociationUseCase
    {
        List<PlayerAssociationDTO> GetAll();
        Task<List<PlayerAssociationDTO>> GetAllAsync();
        Task<PlayerAssociationDTO?> GetAsync(int idPlayerAssociation);
    }
}