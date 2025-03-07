using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCategoryCase
{
    public interface IGetKillerPerkCategoryUseCase
    {
        List<KillerPerkCategoryDTO> GetAll();
        Task<List<KillerPerkCategoryDTO>> GetAllAsync();
        Task<KillerPerkCategoryDTO?> GetAsync(int idKillerPerkCategory);
    }
}