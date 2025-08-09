using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.AssociationCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class AssociationService(ICreateAssociationUseCase createAssociationUseCase,
                                    IDeleteAssociationUseCase deleteAssociationUseCase,
                                    IGetAssociationUseCase getAssociationUseCase,
                                    IUpdateAssociationUseCase updateAssociationUseCase) : IAssociationService
    {
        private readonly ICreateAssociationUseCase _createAssociationUseCase = createAssociationUseCase;
        private readonly IDeleteAssociationUseCase _deleteAssociationUseCase = deleteAssociationUseCase;
        private readonly IGetAssociationUseCase _getAssociationUseCase = getAssociationUseCase;
        private readonly IUpdateAssociationUseCase _updateAssociationUseCase = updateAssociationUseCase;

        public async Task<(PlayerAssociationDTO? PlayerAssociationDTO, string? Message)> CreateAsync(string playerAssociationName, string playerAssociationDescription)
        {
            return await _createAssociationUseCase.CreateAsync(playerAssociationName, playerAssociationDescription);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idPlayerAssociation)
        {
            return await _deleteAssociationUseCase.DeleteAsync(idPlayerAssociation);
        }

        public List<PlayerAssociationDTO> GetAll()
        {
            return _getAssociationUseCase.GetAll();
        }

        public async Task<List<PlayerAssociationDTO>> GetAllAsync()
        {
            return await _getAssociationUseCase.GetAllAsync();
        }

        public async Task<PlayerAssociationDTO?> GetAsync(int idPlayerAssociation)
        {
            return await _getAssociationUseCase.GetAsync(idPlayerAssociation);
        }

        public async Task<(PlayerAssociationDTO? PlayerAssociationDTO, string? Message)> UpdateAsync(int idPlayerAssociation, string playerAssociationName, string playerAssociationDescription)
        {
            return await _updateAssociationUseCase.UpdateAsync(idPlayerAssociation, playerAssociationName, playerAssociationDescription);
        }

        public async Task<PlayerAssociationDTO> ForcedUpdateAsync(int idPlayerAssociation, string playerAssociationName, string playerAssociationDescription)
        {
            return await _updateAssociationUseCase.ForcedUpdateAsync(idPlayerAssociation, playerAssociationName, playerAssociationDescription);
        }
    }
}
