using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface IGameModeService
    {
        Task<(GameModeDTO? GameModeDTO, string? Message)> CreateAsync(string gameModeName, string gameModeDescription);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idGameMode);
        List<GameModeDTO> GetAl();
        Task<List<GameModeDTO>> GetAllAsync();
        Task<GameModeDTO?> GetAsync(int idGameMode);
        Task<(GameModeDTO? GameModeDTO, string? Message)> UpdateAsync(int idGameMode, string gameModeName, string gameModeDescription);
        Task<GameModeDTO> ForcedUpdateAsync(int idGameMode, string gameModeName, string gameModeDescription);
    }
}