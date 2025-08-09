using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.MapCase
{
    public interface IGetMapUseCase
    {
        List<MapDTO> GetAll();
        Task<List<MapDTO>> GetAllAsync();
        Task<MapDTO?> GetAsync(int idMap);
    }
}