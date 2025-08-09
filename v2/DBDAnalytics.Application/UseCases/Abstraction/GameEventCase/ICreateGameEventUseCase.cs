using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.GameEventCase
{
    public interface ICreateGameEventUseCase
    {
        Task<(GameEventDTO? GameEventDTO, string? Message)> CreateAsync(string gameEventName, string gameEventDescription);
    }
}