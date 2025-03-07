using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.GameEventCase
{
    public interface IGetGameEventUseCase
    {
        List<GameEventDTO> GetAll();
        Task<List<GameEventDTO>> GetAllAsync();
        Task<GameEventDTO?> GetAsync(int idGameEvent);
    }
}