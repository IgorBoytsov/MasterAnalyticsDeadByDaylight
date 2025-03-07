using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.KillerAddonCase
{
    public interface IUpdateKillerAddonUseCase
    {
        Task<(KillerAddonDTO? KillerAddonDTO, string? Message)> UpdateAsync(int idAddon, int idKiller, int? idRarity, string addonName, byte[]? addonImage, string? addonDescription);
    }
}