using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface ISurvivorPerkCategoryService
    {
        Task<(SurvivorPerkCategoryDTO? SurvivorPerkCategoryDTO, string? Message)> CreateAsync(string survivorPerkCategoryName, string? description);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idSurvivorPerkCategory);
        List<SurvivorPerkCategoryDTO> GetAll();
        Task<List<SurvivorPerkCategoryDTO>> GetAllAsync();
        Task<SurvivorPerkCategoryDTO> GetAsync(int idSurvivorPerkCategory);
        Task<(SurvivorPerkCategoryDTO? SurvivorPerkCategoryDTO, string? Message)> UpdateAsync(int idSurvivorPerkCategory, string survivorPerkCategoryName, string? description);
    }
}