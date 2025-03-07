using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface IKillerService
    {
        Task<(KillerDTO? KillerrDTO, string Message)> CreateAsync(string killerName, byte[]? killerImage, byte[]? killerAbilityImage);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idKiller);
        Task<KillerDTO> ForcedUpdateAsync(int idKiller, string killerName, byte[]? killerImage, byte[]? killerAbilityImage);
        List<KillerDTO> GetAll();
        Task<List<KillerDTO>> GetAllAsync();
        Task<KillerDTO> GetAsync(int idKiller);
        Task<List<KillerLoadoutDTO>> GetKillersWithAddonsAndPerksAsync();
        Task<KillerLoadoutDTO> GetKillerWithAddonsAndPerksAsync(int idKiller);
        Task<(KillerDTO? KillerDTO, string? Message)> UpdateAsync(int idKiller, string killerName, byte[]? killerImage, byte[]? killerAbilityImage);
    }
}