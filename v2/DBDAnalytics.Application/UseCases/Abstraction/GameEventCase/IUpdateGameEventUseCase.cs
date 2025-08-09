using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.GameEventCase
{
    public interface IUpdateGameEventUseCase
    {
        Task<(GameEventDTO? GameEventDTO, string? Message)> UpdateAsync(int idGameEvent, string gameEventName, string gameEventDescription);
        Task<GameEventDTO?> ForcedUpdateAsync(int idGameEvent, string gameEventName, string gameEventDescription);
    }
}