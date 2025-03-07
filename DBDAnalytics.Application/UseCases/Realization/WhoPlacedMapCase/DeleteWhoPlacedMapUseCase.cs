using DBDAnalytics.Application.UseCases.Abstraction.WhoPlacedMapCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.WhoPlacedMapCase
{
    public class DeleteWhoPlacedMapUseCase(IWhoPlacedMapRepository whoPlacedMapRepository) : IDeleteWhoPlacedMapUseCase
    {
        private readonly IWhoPlacedMapRepository _whoPlacedMapRepository = whoPlacedMapRepository;

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idWhoPlacedMap)
        {
            string message = string.Empty;

            var existBeforeDelete = await _whoPlacedMapRepository.ExistAsync(idWhoPlacedMap);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _whoPlacedMapRepository.DeleteAsync(idWhoPlacedMap);

            var existAfterDelete = await _whoPlacedMapRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}
