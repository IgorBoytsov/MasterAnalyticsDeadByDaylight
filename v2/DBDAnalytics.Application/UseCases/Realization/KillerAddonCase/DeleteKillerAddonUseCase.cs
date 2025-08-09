using DBDAnalytics.Application.UseCases.Abstraction.KillerAddonCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.KillerAddonCase
{
    public class DeleteKillerAddonUseCase(IKillerAddonRepository killerAddonRepository) : IDeleteKillerAddonUseCase
    {
        private readonly IKillerAddonRepository _itemAddonRepository = killerAddonRepository;

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idKillerAddon)
        {
            string message = string.Empty;

            var existBeforeDelete = await _itemAddonRepository.ExistAsync(idKillerAddon);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _itemAddonRepository.DeleteAsync(idKillerAddon);

            var existAfterDelete = await _itemAddonRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}