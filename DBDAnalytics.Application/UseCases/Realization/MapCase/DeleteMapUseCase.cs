using DBDAnalytics.Application.UseCases.Abstraction.MapCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.MapCase
{
    public class DeleteMapUseCase(IMapRepository mapRepository) : IDeleteMapUseCase
    {
        private readonly IMapRepository _mapRepository = mapRepository;

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idMap)
        {
            string message = string.Empty;

            var existBeforeDelete = await _mapRepository.ExistAsync(idMap);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _mapRepository.DeleteAsync(idMap);

            var existAfterDelete = await _mapRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}