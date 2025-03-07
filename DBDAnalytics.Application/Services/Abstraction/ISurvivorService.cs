using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface ISurvivorService
    {
        Task<(SurvivorDTO? SurvivorDTO, string Message)> CreateAsync(string survivorName, byte[]? survivorImage, string? survivorDescription);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idSurvivor);
        Task<SurvivorDTO> ForcedUpdateAsync(int idSurvivor, string survivorName, byte[]? survivorImage, string? survivorDescription);
        List<SurvivorDTO> GetAll();
        Task<List<SurvivorDTO>> GetAllAsync();
        Task<SurvivorDTO> GetAsync(int idSurvivor);
        Task<List<SurvivorWithPerksDTO>> GetSurvivorsWithPerksAsync();
        Task<SurvivorWithPerksDTO> GetSurvivorWithPerksAsync(int idSurvivor);
        Task<(SurvivorDTO? SurvivorDTO, string? Message)> UpdateAsync(int idSurvivor, string survivorName, byte[]? survivorImage, string? survivorDescription);
    }
}