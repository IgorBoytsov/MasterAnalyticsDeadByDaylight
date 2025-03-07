using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface IGameEventService
    {
        Task<(GameEventDTO? GameEventDTO, string? Message)> CreateAsync(string gameEventName, string gameEventDescription);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idGameEvent);
        List<GameEventDTO> GetAll();
        Task<List<GameEventDTO>> GetAllAsync();
        Task<GameEventDTO?> GetAsync(int idGameEvent);
        Task<(GameEventDTO? GameEventDTO, string? Message)> UpdateAsync(int idGameEvent, string gameEventName, string gameEventDescription);
        Task<GameEventDTO> ForcedUpdateAsync(int idGameEvent, string gameEventName, string gameEventDescription);
    }
}