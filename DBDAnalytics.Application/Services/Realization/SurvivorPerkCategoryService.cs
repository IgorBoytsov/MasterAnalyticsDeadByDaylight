using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCategoryCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class SurvivorPerkCategoryService(ICreateSurvivorPerkCategoryUseCase createSurvivorPerkCategoryUseCase,
                                             IDeleteSurvivorPerkCategoryUseCase deleteSurvivorPerkCategoryUseCase,
                                             IGetSurvivorPerkCategoryUseCase getSurvivorPerkCategoryUseCase,
                                             IUpdateSurvivorPerkCategoryUseCase updateSurvivorPerkCategoryUseCase) : ISurvivorPerkCategoryService
    {
        private readonly ICreateSurvivorPerkCategoryUseCase _createSurvivorPerkCategoryUseCase = createSurvivorPerkCategoryUseCase;
        private readonly IDeleteSurvivorPerkCategoryUseCase _deleteSurvivorPerkCategoryUseCase = deleteSurvivorPerkCategoryUseCase;
        private readonly IGetSurvivorPerkCategoryUseCase _getSurvivorPerkCategoryUseCase = getSurvivorPerkCategoryUseCase;
        private readonly IUpdateSurvivorPerkCategoryUseCase _updateSurvivorPerkCategoryUseCase = updateSurvivorPerkCategoryUseCase;

        public async Task<(SurvivorPerkCategoryDTO? SurvivorPerkCategoryDTO, string Message)> CreateAsync(string survivorPerkCategoryName)
        {
            return await _createSurvivorPerkCategoryUseCase.CreateAsync(survivorPerkCategoryName);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idSurvivorPerkCategory)
        {
            return await _deleteSurvivorPerkCategoryUseCase.DeleteAsync(idSurvivorPerkCategory);
        }

        public List<SurvivorPerkCategoryDTO> GetAll()
        {
            return _getSurvivorPerkCategoryUseCase.Get();
        }

        public async Task<List<SurvivorPerkCategoryDTO>> GetAllAsync()
        {
            return await _getSurvivorPerkCategoryUseCase.GetAllAsync();
        }

        public async Task<SurvivorPerkCategoryDTO> GetAsync(int idSurvivorPerkCategory)
        {
            return await _getSurvivorPerkCategoryUseCase.GetAsync(idSurvivorPerkCategory);
        }

        public async Task<(SurvivorPerkCategoryDTO? SurvivorPerkCategoryDTO, string? Message)> UpdateAsync(int idSurvivorPerkCategory, string survivorPerkCategoryName)
        {
            return await _updateSurvivorPerkCategoryUseCase.UpdateAsync(idSurvivorPerkCategory, survivorPerkCategoryName);
        }
    }
}
