using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.SurvivorCase
{
    public interface ICreateSurvivorUseCase
    {
        Task<(SurvivorDTO? SurvivorDTO, string Message)> CreateAsync(string survivorName, byte[]? survivorImage, string? survivorDescription);
    }
}