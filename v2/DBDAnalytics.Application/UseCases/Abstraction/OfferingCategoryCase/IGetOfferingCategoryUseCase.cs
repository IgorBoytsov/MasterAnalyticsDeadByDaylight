using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.OfferingCategoryCase
{
    public interface IGetOfferingCategoryUseCase
    {
        List<OfferingCategoryDTO> GetAll();
        Task<List<OfferingCategoryDTO>> GetAllAsync();
        Task<OfferingCategoryDTO?> GetAsync(int idOfferingCategory);
    }
}