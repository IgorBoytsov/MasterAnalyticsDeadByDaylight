using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface IRarityService
    {
        Task<(RarityDTO? RarityDTO, string Message)> CreateAsync(string rarityName);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idRarity);
        List<RarityDTO> GetAll();
        Task<List<RarityDTO>> GetAllAsync();
        Task<RarityDTO> GetAsync(int idRarity);
        Task<(RarityDTO? RarityDTO, string? Message)> UpdateAsync(int idRarity, string rarityName);
    }
}