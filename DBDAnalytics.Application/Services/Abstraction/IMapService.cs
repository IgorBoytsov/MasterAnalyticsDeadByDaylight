using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface IMapService
    {
        Task<(MapDTO? MapDTO, string? Message)> CreateAsync(int idMeasurement, string mapName, byte[] mapImage, string mapDescription);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idMap);
        Task<MapDTO> ForcedUpdateAsync(int idMap, int idMeasurement, string mapName, byte[] mapImage, string mapDescription);
        List<MapDTO> GetAll();
        Task<List<MapDTO>> GetAllAsync();
        Task<MapDTO> GetAsync(int idMap);
        Task<(MapDTO? MapDTO, string? Message)> UpdateAsync(int idMap, int idMeasurement, string mapName, byte[] mapImage, string mapDescription);
    }
}