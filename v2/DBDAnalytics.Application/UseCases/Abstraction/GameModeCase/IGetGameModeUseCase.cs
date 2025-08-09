using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.GameModeCase
{
    public interface IGetGameModeUseCase
    {
        List<GameModeDTO> GetAll();
        Task<List<GameModeDTO>> GetAllAsync();
        Task<GameModeDTO?> GetAsync(int idGameMode);
    }
}