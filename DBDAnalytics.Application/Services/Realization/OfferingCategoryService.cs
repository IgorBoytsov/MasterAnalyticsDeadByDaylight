using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.OfferingCategoryCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class OfferingCategoryService(ICreateOfferingCategoryUseCase createOfferingCategoryUseCase,
                                         IDeleteOfferingCategoryUseCase deleteOfferingCategoryUseCase,
                                         IGetOfferingCategoryUseCase getOfferingCategoryUseCase,
                                         IUpdateOfferingCategoryUseCase updateOfferingCategoryUseCase) : IOfferingCategoryService
    {
        private readonly ICreateOfferingCategoryUseCase _createOfferingCategoryUseCase = createOfferingCategoryUseCase;
        private readonly IDeleteOfferingCategoryUseCase _deleteOfferingCategoryUseCase = deleteOfferingCategoryUseCase;
        private readonly IGetOfferingCategoryUseCase _getOfferingCategoryUseCase = getOfferingCategoryUseCase;
        private readonly IUpdateOfferingCategoryUseCase _updateOfferingCategoryUseCase = updateOfferingCategoryUseCase;

        public async Task<(OfferingCategoryDTO? OfferingCategoryDTO, string? Message)> CreateAsync(string offeringCategoryName, string? description)
        {
            return await _createOfferingCategoryUseCase.CreateAsync(offeringCategoryName, description);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idOfferingCategory)
        {
            return await _deleteOfferingCategoryUseCase.DeleteAsync(idOfferingCategory);
        }

        public List<OfferingCategoryDTO> GetAll()
        {
            return _getOfferingCategoryUseCase.GetAll();
        }

        public async Task<List<OfferingCategoryDTO>> GetAllAsync()
        {
            return await _getOfferingCategoryUseCase.GetAllAsync();
        }

        public async Task<OfferingCategoryDTO> GetAsync(int idOfferingCategory)
        {
            return await _getOfferingCategoryUseCase.GetAsync(idOfferingCategory);
        }

        public async Task<(OfferingCategoryDTO? KillerPerkCategoryDTO, string? Message)> UpdateAsync(int idOfferingCategory, string offeringCategoryName, string? description)
        {
            return await _updateOfferingCategoryUseCase.UpdateAsync(idOfferingCategory, offeringCategoryName, description);
        }
    }
}