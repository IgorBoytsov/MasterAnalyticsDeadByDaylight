using DBDAnalytics.Application.UseCases.Abstraction.GameEventCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.GameEventCase
{
    public class DeleteGameEventUseCase(IGameEventRepository gameEventRepository) : IDeleteGameEventUseCase
    {
        private readonly IGameEventRepository _gameEventRepository = gameEventRepository;

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idGameEvent)
        {
            string message = string.Empty;

            var existBeforeDelete = await _gameEventRepository.ExistAsync(idGameEvent);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _gameEventRepository.DeleteAsync(idGameEvent);

            var existAfterDelete = await _gameEventRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}
