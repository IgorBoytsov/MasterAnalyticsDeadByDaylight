using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.MapCase
{
    public interface ICreateMapUseCase
    {
        Task<(MapDTO? MapDTO, string? Message)> CreateAsync(int idMeasurement, string mapName, byte[] mapImage, string mapDescription);
    }
}