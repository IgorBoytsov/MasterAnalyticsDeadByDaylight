using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface ISurvivorPerkService
    {
        Task<(SurvivorPerkDTO? SurvivorPerkDTO, string? Message)> CreateAsync(int idSurvivor, string perkName, byte[]? perkImage, int? idCategory, string? perkDescription);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idPerk);
        List<SurvivorPerkDTO> GetAll();
        Task<List<SurvivorPerkDTO>> GetAllAsync();
        Task<SurvivorPerkDTO> GetAsync(int idPerk);
        Task<(SurvivorPerkDTO? SurvivorPerkDTO, string? Message)> UpdateAsync(int idPerk, int idSurvivor, string perkName, byte[]? perkImage, int? idCategory, string? perkDescription);
    }
}