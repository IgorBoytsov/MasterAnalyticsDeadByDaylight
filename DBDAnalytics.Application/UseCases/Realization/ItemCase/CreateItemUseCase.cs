using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.ItemCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.ItemCase
{
    public class CreateItemUseCase(IItemRepository itemRepository) : ICreateItemUseCase
    {
        private readonly IItemRepository _itemRepository = itemRepository;

        public async Task<(ItemDTO? ItemDTO, string? Message)> CreateAsync(string itemName, byte[]? itemImage, string? itemDescription)
        {
            string message = string.Empty;

            var (CreatedItem, Message) = ItemDomain.Create(0, itemName, itemImage, itemDescription);

            if (CreatedItem is null)
            {
                return (null, Message);
            }

            bool exist = await _itemRepository.ExistAsync(itemName);

            if (exist)
                return (null, "Запись с таким названием уже существует.");

            var id = await _itemRepository.CreateAsync(itemName, itemImage, itemDescription);

            var domainEntity = await _itemRepository.GetAsync(id);

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}