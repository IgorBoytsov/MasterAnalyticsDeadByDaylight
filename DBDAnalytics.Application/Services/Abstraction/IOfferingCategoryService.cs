using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface IOfferingCategoryService
    {
        Task<(OfferingCategoryDTO? OfferingCategoryDTO, string? Message)> CreateAsync(string offeringCategoryName);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idOfferingCategory);
        List<OfferingCategoryDTO> GetAll();
        Task<List<OfferingCategoryDTO>> GetAllAsync();
        Task<OfferingCategoryDTO> GetAsync(int idOfferingCategory);
        Task<(OfferingCategoryDTO? KillerPerkCategoryDTO, string? Message)> UpdateAsync(int idOfferingCategory, string offeringCategoryName);
    }
}