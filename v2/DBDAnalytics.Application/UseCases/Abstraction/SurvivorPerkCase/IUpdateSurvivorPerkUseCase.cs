using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCase
{
    public interface IUpdateSurvivorPerkUseCase
    {
        Task<(SurvivorPerkDTO? SurvivorPerkDTO, string? Message)> UpdateAsync(int idPerk, int idSurvivor, string perkName, byte[]? perkImage, int? idCategory, string? perkDescription);
    }
}