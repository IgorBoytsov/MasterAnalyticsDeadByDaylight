using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.PlatformCase
{
    public interface IGetPlatformUseCase
    {
        List<PlatformDTO> GetAll();
        Task<List<PlatformDTO>> GetAllAsync();
        Task<PlatformDTO?> GetAsync(int idPlatform);
    }
}