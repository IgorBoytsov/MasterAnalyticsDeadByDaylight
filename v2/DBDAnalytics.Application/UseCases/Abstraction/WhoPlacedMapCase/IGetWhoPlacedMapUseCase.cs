using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.WhoPlacedMapCase
{
    public interface IGetWhoPlacedMapUseCase
    {
        List<WhoPlacedMapDTO> GetAll();
        Task<List<WhoPlacedMapDTO>> GetAllAsync();
        Task<WhoPlacedMapDTO?> GetAsync(int idWhoPlacedMap);
    }
}