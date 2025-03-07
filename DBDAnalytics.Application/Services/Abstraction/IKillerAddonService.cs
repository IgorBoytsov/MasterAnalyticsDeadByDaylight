using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface IKillerAddonService
    {
        Task<(KillerAddonDTO? KillerAddonDTO, string Message)> CreateAsync(int idKiller, int? idRarity, string addonName, byte[]? addonImage, string? addonDescription);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idKillerAddon);
        List<KillerAddonDTO> GetAll();
        Task<List<KillerAddonDTO>> GetAllAsync();
        Task<KillerAddonDTO> GetAsync(int idKillerAddon);
        Task<(KillerAddonDTO? KillerAddonDTO, string? Message)> UpdateAsync(int idAddon, int idKiller, int? idRarity, string addonName, byte[]? addonImage, string? addonDescription);
    }
}