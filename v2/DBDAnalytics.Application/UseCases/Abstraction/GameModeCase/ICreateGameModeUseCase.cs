using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.GameModeCase
{
    public interface ICreateGameModeUseCase
    {
        Task<(GameModeDTO? GameModeDTO, string? Message)> CreateAsync(string gameModeName, string gameModeDescription);
    }
}