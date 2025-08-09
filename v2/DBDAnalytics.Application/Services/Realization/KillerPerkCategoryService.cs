using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCategoryCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class KillerPerkCategoryService(ICreateKillerPerkCategoryUseCase createKillerPerkCategoryUseCase,
                                           IDeleteKillerPerkCategoryUseCase deleteKillerPerkCategoryUseCase,
                                           IGetKillerPerkCategoryUseCase getKillerPerkCategoryUseCase,
                                           IUpdateKillerPerkCategoryUseCase updateKillerPerkCategoryUseCase) : IKillerPerkCategoryService
    {
        private readonly ICreateKillerPerkCategoryUseCase _createKillerPerkCategoryUseCase = createKillerPerkCategoryUseCase;
        private readonly IDeleteKillerPerkCategoryUseCase _deleteKillerPerkCategoryUseCase = deleteKillerPerkCategoryUseCase;
        private readonly IGetKillerPerkCategoryUseCase _getKillerPerkCategoryUseCase = getKillerPerkCategoryUseCase;
        private readonly IUpdateKillerPerkCategoryUseCase _updateKillerPerkCategoryUseCase = updateKillerPerkCategoryUseCase;

        public async Task<(KillerPerkCategoryDTO? KillerPerkCategoryDTO, string? Message)> CreateAsync(string killerPerkCategoryName, string? description)
        {
            return await _createKillerPerkCategoryUseCase.CreateAsync(killerPerkCategoryName, description);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idKillerPerkCategory)
        {
            return await _deleteKillerPerkCategoryUseCase.DeleteAsync(idKillerPerkCategory);
        }

        public List<KillerPerkCategoryDTO> GetAll()
        {
            return _getKillerPerkCategoryUseCase.GetAll();
        }

        public async Task<List<KillerPerkCategoryDTO>> GetAllAsync()
        {
            return await _getKillerPerkCategoryUseCase.GetAllAsync();
        }

        public async Task<KillerPerkCategoryDTO> GetAsync(int idKillerPerkCategory)
        {
            return await _getKillerPerkCategoryUseCase.GetAsync(idKillerPerkCategory);
        }

        public async Task<(KillerPerkCategoryDTO? KillerPerkCategoryDTO, string? Message)> UpdateAsync(int idKillerPerkCategory, string killerPerkCategoryName, string? description)
        {
            return await _updateKillerPerkCategoryUseCase.UpdateAsync(idKillerPerkCategory, killerPerkCategoryName, description);
        }
    }
}