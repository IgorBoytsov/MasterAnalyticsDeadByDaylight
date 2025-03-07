using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.GameModeCase
{
    public interface IUpdateGameModeUseCase
    {
        Task<(GameModeDTO? GameEventDTO, string? Message)> UpdateAsync(int idGameMode, string gameModeName, string gameModeDescription);
        Task<GameModeDTO?> ForcedUpdateAsync(int idGameMode, string gameModeName, string gameModeDescription);
    }
}