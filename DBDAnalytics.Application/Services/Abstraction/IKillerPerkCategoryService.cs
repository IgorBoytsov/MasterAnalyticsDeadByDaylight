using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface IKillerPerkCategoryService
    {
        Task<(KillerPerkCategoryDTO? KillerPerkCategoryDTO, string Message)> CreateAsync(string killerPerkCategoryName);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idKillerPerkCategory);
        List<KillerPerkCategoryDTO> GetAll();
        Task<List<KillerPerkCategoryDTO>> GetAllAsync();
        Task<KillerPerkCategoryDTO> GetAsync(int idKillerPerkCategory);
        Task<(KillerPerkCategoryDTO? KillerPerkCategoryDTO, string? Message)> UpdateAsync(int idKillerPerkCategory, string killerPerkCategoryName);
    }
}