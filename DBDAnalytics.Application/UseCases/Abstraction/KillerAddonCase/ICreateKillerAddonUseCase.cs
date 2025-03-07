using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.KillerAddonCase
{
    public interface ICreateKillerAddonUseCase
    {
        Task<(KillerAddonDTO? KillerAddonDTO, string? Message)> CreateAsync(int idKiller, int? idRarity, string addonName, byte[]? addonImage, string? addonDescription);
    }
}