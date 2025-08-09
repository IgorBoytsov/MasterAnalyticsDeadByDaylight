using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCase
{
    public interface ICreateSurvivorPerkUseCase
    {
        Task<(SurvivorPerkDTO? SurvivorPerkDTO, string? Message)> CreateAsync(int idSurvivor, string perkName, byte[]? perkImage, int? idCategory, string? perkDescription);
    }
}