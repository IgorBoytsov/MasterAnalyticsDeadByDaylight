using DBDAnalytics.Application.UseCases.Abstraction.GameModeCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.GameModeCase
{
    public class DeleteGameModeUseCase(IGameModeRepository gameModeRepository) : IDeleteGameModeUseCase
    {
        private readonly IGameModeRepository _gameModeRepository = gameModeRepository;

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idGameMode)
        {
            string message = string.Empty;

            var existBeforeDelete = await _gameModeRepository.ExistAsync(idGameMode);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _gameModeRepository.DeleteAsync(idGameMode);

            var existAfterDelete = await _gameModeRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}
