using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.GameModeCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class GameModeService(ICreateGameModeUseCase createGameModeUseCase,
                                 IDeleteGameModeUseCase deleteGameModeUseCase,
                                 IGetGameModeUseCase getGameModeUseCase,
                                 IUpdateGameModeUseCase updateGameModeUseCase) : IGameModeService
    {
        private readonly ICreateGameModeUseCase _createGameModeUseCase = createGameModeUseCase;
        private readonly IDeleteGameModeUseCase _deleteGameModeUseCase = deleteGameModeUseCase;
        private readonly IGetGameModeUseCase _getGameModeUseCase = getGameModeUseCase;
        private readonly IUpdateGameModeUseCase _updateGameModeUseCase = updateGameModeUseCase;

        public async Task<(GameModeDTO? GameModeDTO, string? Message)> CreateAsync(string gameModeName, string gameModeDescription)
        {
            return await _createGameModeUseCase.CreateAsync(gameModeName, gameModeDescription);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idGameMode)
        {
            return await _deleteGameModeUseCase.DeleteAsync(idGameMode);
        }

        public List<GameModeDTO> GetAl()
        {
            return _getGameModeUseCase.GetAll();
        }

        public async Task<List<GameModeDTO>> GetAllAsync()
        {
            return await _getGameModeUseCase.GetAllAsync();
        }

        public async Task<GameModeDTO?> GetAsync(int idGameMode)
        {
            return await _getGameModeUseCase.GetAsync(idGameMode);
        }

        public async Task<(GameModeDTO? GameModeDTO, string? Message)> UpdateAsync(int idGameMode, string gameModeName, string gameModeDescription)
        {
            return await _updateGameModeUseCase.UpdateAsync(idGameMode, gameModeName, gameModeDescription);
        }

        public async Task<GameModeDTO> ForcedUpdateAsync(int idGameMode, string gameModeName, string gameModeDescription)
        {
            return await _updateGameModeUseCase.ForcedUpdateAsync(idGameMode, gameModeName, gameModeDescription);
        }
    }
}
