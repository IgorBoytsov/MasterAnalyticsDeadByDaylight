using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.SurvivorCase
{
    public interface IUpdateSurvivorUseCase
    {
        Task<SurvivorDTO?> ForcedUpdateAsync(int idSurvivor, string survivorName, byte[]? survivorImage, string? survivorDescription);
        Task<(SurvivorDTO? SurvivorDTO, string? Message)> UpdateAsync(int idSurvivor, string survivorName, byte[]? survivorImage, string? survivorDescription);
    }
}