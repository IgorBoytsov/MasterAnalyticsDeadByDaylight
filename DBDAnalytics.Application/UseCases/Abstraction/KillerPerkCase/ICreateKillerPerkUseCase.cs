using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCase
{
    public interface ICreateKillerPerkUseCase
    {
        Task<(KillerPerkDTO? KillerPerkDTO, string? Message)> CreateAsync(int idKiller, string perkName, byte[]? perkImage, int? idCategory, string? perkDescription);
    }
}