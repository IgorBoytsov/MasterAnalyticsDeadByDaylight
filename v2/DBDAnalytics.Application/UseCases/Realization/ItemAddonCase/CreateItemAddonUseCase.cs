using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.ItemAddonCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.ItemAddonCase
{
    public class CreateItemAddonUseCase(IItemAddonRepository itemAddonRepository) : ICreateItemAddonUseCase
    {
        private readonly IItemAddonRepository _itemAddonRepository = itemAddonRepository;

        public async Task<(ItemAddonDTO? ItemAddonDTO, string? Message)> CreateAsync(int idItem, int? idRarity, string itemAddonName, byte[]? itemAddonImage, string? itemAddonDescription)
        {
            string message = string.Empty;

            var (CreatedItemAddon, Message) = ItemAddonDomain.Create(0, idItem, idRarity, itemAddonName, itemAddonImage, itemAddonDescription);

            if (CreatedItemAddon is null)
            {
                return (null, Message);
            }

            var id = await _itemAddonRepository.CreateAsync(idItem, idRarity, itemAddonName, itemAddonImage, itemAddonDescription);

            var domainEntity = await _itemAddonRepository.GetAsync(id);

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}