using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.GameEventCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class GameEventService(ICreateGameEventUseCase createGameEventUseCase,
                                  IDeleteGameEventUseCase deleteGameEventUseCase,
                                  IGetGameEventUseCase getGameEventUseCase,
                                  IUpdateGameEventUseCase updateGameEventUseCase) : IGameEventService
    {
        private readonly ICreateGameEventUseCase _createGameEventUseCase = createGameEventUseCase;
        private readonly IDeleteGameEventUseCase _deleteGameEventUseCase = deleteGameEventUseCase;
        private readonly IGetGameEventUseCase _getGameEventUseCase = getGameEventUseCase;
        private readonly IUpdateGameEventUseCase _updateGameEventUseCase = updateGameEventUseCase;

        public async Task<(GameEventDTO? GameEventDTO, string? Message)> CreateAsync(string gameEventName, string gameEventDescription)
        {
            return await _createGameEventUseCase.CreateAsync(gameEventName, gameEventDescription);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idGameEvent)
        {
            return await _deleteGameEventUseCase.DeleteAsync(idGameEvent);
        }

        public List<GameEventDTO> GetAll()
        {
            return _getGameEventUseCase.GetAll();
        }

        public async Task<List<GameEventDTO>> GetAllAsync()
        {
            return await _getGameEventUseCase.GetAllAsync();
        }

        public async Task<GameEventDTO?> GetAsync(int idGameEvent)
        {
            return await _getGameEventUseCase.GetAsync(idGameEvent);
        }

        public async Task<(GameEventDTO? GameEventDTO, string? Message)> UpdateAsync(int idGameEvent, string gameEventName, string gameEventDescription)
        {
            return await _updateGameEventUseCase.UpdateAsync(idGameEvent, gameEventName, gameEventDescription);
        }

        public async Task<GameEventDTO> ForcedUpdateAsync(int idGameEvent, string gameEventName, string gameEventDescription)
        {
            return await _updateGameEventUseCase.ForcedUpdateAsync(idGameEvent, gameEventName, gameEventDescription);
        }
    }
}
