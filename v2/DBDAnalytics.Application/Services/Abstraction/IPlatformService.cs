using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface IPlatformService
    {
        Task<(PlatformDTO? PlatformDTO, string Message)> CreateAsync(string platformName, string platformDescription);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idPlatform);
        List<PlatformDTO> GetAll();
        Task<List<PlatformDTO>> GetAllAsync();
        Task<PlatformDTO> GetAsync(int idPlatform);
        Task<(PlatformDTO? PlatformDTO, string? Message)> UpdateAsync(int idPlatform, string platformName, string platformDescription);
        Task<PlatformDTO> ForcedUpdateAsync(int idPlatform, string platformName, string platformDescription);
    }
}