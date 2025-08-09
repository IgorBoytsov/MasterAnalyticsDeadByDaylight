using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCase
{
    public interface IUpdateKillerPerkUseCase
    {
        Task<(KillerPerkDTO? KillerPerkDTO, string? Message)> UpdateAsync(int idPerk, int idKiller, string perkName, byte[]? perkImage, int? idCategory, string? perkDescription);
    }
}