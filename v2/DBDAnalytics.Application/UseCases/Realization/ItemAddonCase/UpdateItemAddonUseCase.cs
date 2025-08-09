using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.ItemAddonCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.ItemAddonCase
{
    public class UpdateItemAddonUseCase(IItemAddonRepository itemAddonRepository) : IUpdateItemAddonUseCase
    {
        private readonly IItemAddonRepository _itemAddonRepository = itemAddonRepository;

        public async Task<(ItemAddonDTO? ItemAddonDTO, string? Message)> UpdateAsync(int idItemAddon, int idItem, int? idRarity, string itemAddonName, byte[]? itemAddonImage, string? itemAddonDescription)
        {
            string message = string.Empty;

            if (idItem == 0)
                return (null, "Вы не выбрали к какому предмету относиться улучшение.");

            if (idItem == 0 || string.IsNullOrWhiteSpace(itemAddonName))
                return (null, "Такой записи не существует");

            int id = await _itemAddonRepository.UpdateAsync(idItemAddon, idItem, idRarity, itemAddonName, itemAddonImage, itemAddonDescription);

            var domainEntity = await _itemAddonRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}