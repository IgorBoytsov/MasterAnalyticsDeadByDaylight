using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.PlatformCase
{
    public interface IUpdatePlatformUseCase
    {
        Task<(PlatformDTO? PlatformDTO, string? Message)> UpdateAsync(int idPlatform, string platformName, string platformDescription);
        Task<PlatformDTO?> ForcedUpdateAsync(int idPlatform, string platformName, string platformDescription);
    }
}