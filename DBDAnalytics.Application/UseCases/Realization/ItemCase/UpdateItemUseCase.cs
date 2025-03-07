using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.ItemCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.ItemCase
{
    public class UpdateItemUseCase(IItemRepository itemRepository) : IUpdateItemUseCase
    {
        private readonly IItemRepository _itemRepository = itemRepository;

        public async Task<(ItemDTO? ItemDTO, string? Message)> UpdateAsync(int idItem, string itemName, byte[]? itemImage, string? itemDescription)
        {
            string message = string.Empty;

            if (idItem == 0 || string.IsNullOrWhiteSpace(itemName))
                return (null, "Такой записи не существует");

            var exist = await _itemRepository.ExistAsync(itemName);

            if (exist)
                return (null, "Название на которое вы хотите поменять - уже существует.");

            int id = await _itemRepository.UpdateAsync(idItem, itemName, itemImage, itemDescription);

            var domainEntity = await _itemRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }

        public async Task<ItemDTO> ForcedUpdateAsync(int idItem, string itemName, byte[]? itemImage, string? itemDescription)
        {
            int id = await _itemRepository.UpdateAsync(idItem, itemName, itemImage, itemDescription);

            var domainEntity = await _itemRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}