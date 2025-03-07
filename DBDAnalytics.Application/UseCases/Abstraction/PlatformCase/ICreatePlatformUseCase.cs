using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.PlatformCase
{
    public interface ICreatePlatformUseCase
    {
        Task<(PlatformDTO? PlatformDTO, string? Message)> CreateAsync(string platformName, string platformDescription);
    }
}