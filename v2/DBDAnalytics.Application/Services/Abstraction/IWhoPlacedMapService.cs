using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface IWhoPlacedMapService
    {
        Task<(WhoPlacedMapDTO? WhoPlacedMapDTO, string? Message)> CreateAsync(string whoPlacedMapName, string? description);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idWhoPlacedMap);
        List<WhoPlacedMapDTO> GetAll();
        Task<List<WhoPlacedMapDTO>> GetAllAsync();
        Task<WhoPlacedMapDTO> GetAsync(int idWhoPlacedMap);
        Task<(WhoPlacedMapDTO? WhoPlacedMapDTO, string? Message)> UpdateAsync(int idWhoPlacedMap, string whoPlacedMapName, string? description);
    }
}