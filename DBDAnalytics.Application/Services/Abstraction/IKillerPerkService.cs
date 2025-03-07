using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface IKillerPerkService
    {
        Task<(KillerPerkDTO? KillerPerkDTO, string? Message)> CreateAsync(int idKiller, string perkName, byte[]? perkImage, int? idCategory, string? perkDescription);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idPerk);
        List<KillerPerkDTO> GetAll();
        Task<List<KillerPerkDTO>> GetAllAsync();
        Task<KillerPerkDTO> GetAsync(int idPerk);
        Task<(KillerPerkDTO? KillerPerkDTO, string? Message)> UpdateAsync(int idPerk, int idKiller, string perkName, byte[]? perkImage, int? idCategory, string? perkDescription);
    }
}