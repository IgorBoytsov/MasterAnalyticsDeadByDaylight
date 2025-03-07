using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCategoryCase
{
    public interface IGetSurvivorPerkCategoryUseCase
    {
        List<SurvivorPerkCategoryDTO> Get();
        Task<List<SurvivorPerkCategoryDTO>> GetAllAsync();
        Task<SurvivorPerkCategoryDTO?> GetAsync(int idSurvivorPerkCategory);
    }
}