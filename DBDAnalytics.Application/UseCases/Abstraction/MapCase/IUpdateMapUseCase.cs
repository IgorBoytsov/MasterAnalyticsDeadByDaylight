using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.MapCase
{
    public interface IUpdateMapUseCase
    {
        Task<MapDTO?> ForcedUpdateAsync(int idMap, int idMeasurement, string mapName, byte[] mapImage, string mapDescription);
        Task<(MapDTO? MapDTO, string? Message)> UpdateAsync(int idMap, int idMeasurement, string mapName, byte[] mapImage, string mapDescription);
    }
}