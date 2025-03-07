using DBDAnalytics.Application.UseCases.Abstraction.MatchAttributeCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.MatchAttributeCase
{
    public class DeleteMatchAttributeUseCase(IMatchAttributeRepository matchAttributeRepository) : IDeleteMatchAttributeUseCase
    {
        private readonly IMatchAttributeRepository _matchAttributeRepository = matchAttributeRepository;

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idMap)
        {
            string message = string.Empty;

            var existBeforeDelete = await _matchAttributeRepository.ExistAsync(idMap);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _matchAttributeRepository.DeleteAsync(idMap);

            var existAfterDelete = await _matchAttributeRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}