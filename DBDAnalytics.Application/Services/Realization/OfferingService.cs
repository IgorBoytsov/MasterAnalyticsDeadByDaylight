using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.OfferingCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class OfferingService(ICreateOfferingUseCase createOfferingUseCase,
                                 IDeleteOfferingUseCase deleteOfferingUseCase,
                                 IGetOfferingUseCase getOfferingUseCase,
                                 IUpdateOfferingUseCase updateOfferingUseCase) : IOfferingService
    {
        private readonly ICreateOfferingUseCase _createOfferingUseCase = createOfferingUseCase;
        private readonly IDeleteOfferingUseCase _deleteOfferingUseCase = deleteOfferingUseCase;
        private readonly IGetOfferingUseCase _getOfferingUseCase = getOfferingUseCase;
        private readonly IUpdateOfferingUseCase _updateOfferingUseCase = updateOfferingUseCase;

        public async Task<(OfferingDTO? OfferingDTO, string Message)> CreateAsync(int idRole, int idCategory, int idRarity, string offeringName, byte[]? offeringImage, string? offeringDescription)
        {
            return await _createOfferingUseCase.CreateAsync(idRole, idCategory, idRarity, offeringName, offeringImage, offeringDescription);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idOffering)
        {
            return await _deleteOfferingUseCase.DeleteAsync(idOffering);
        }

        public List<OfferingDTO> GetAll()
        {
            return _getOfferingUseCase.GetAll();
        }

        public async Task<List<OfferingDTO>> GetAllAsync()
        {
            return await _getOfferingUseCase.GetAllAsync();
        }

        public async Task<OfferingDTO> GetAsync(int idOffering)
        {
            return await _getOfferingUseCase.GetAsync(idOffering);
        }

        public async Task<(OfferingDTO? OfferingDTO, string? Message)> UpdateAsync(int idOffering, int idRole, int idCategory, int idRarity, string offeringName, byte[]? offeringImage, string? offeringDescription)
        {
            return await _updateOfferingUseCase.UpdateAsync(idOffering, idRole, idCategory, idRarity, offeringName, offeringImage, offeringDescription);
        }

        public async Task<OfferingDTO> ForcedUpdateAsync(int idOffering, int idRole, int idCategory, int idRarity, string offeringName, byte[]? offeringImage, string? offeringDescription)
        {
            return await _updateOfferingUseCase.ForcedUpdateAsync(idOffering, idRole, idCategory, idRarity, offeringName, offeringImage, offeringDescription);
        }
    }
}