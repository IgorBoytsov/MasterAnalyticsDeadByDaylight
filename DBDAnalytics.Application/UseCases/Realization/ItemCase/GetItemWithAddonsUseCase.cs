using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.ItemCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.ItemCase
{
    public class GetItemWithAddonsUseCase(IItemRepository itemRepository) : IGetItemWithAddonsUseCase
    {
        private readonly IItemRepository _itemRepository = itemRepository;

        public List<ItemWithAddonsDTO> GetItemsWithAddons()
        {
            var domainEntities = _itemRepository.GetItemsWithAddons();

            if (domainEntities == null)
            {
                return null;
            }

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<List<ItemWithAddonsDTO>> GetItemsWithAddonsAsync()
        {
            var domainEntities = await _itemRepository.GetItemsWithAddonsAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public ItemWithAddonsDTO GetItemWithAddons(int idItem)
        {
            var domainEntities = _itemRepository.GetItemWithAddons(idItem);

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<ItemWithAddonsDTO> GetItemWithAddonsAsync(int idItem)
        {
            var domainEntities = await _itemRepository.GetItemWithAddonsAsync(idItem);

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }
    }
}