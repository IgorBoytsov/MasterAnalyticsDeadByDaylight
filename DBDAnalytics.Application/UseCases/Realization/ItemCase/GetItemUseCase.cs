using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.ItemCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.ItemCase
{
    public class GetItemUseCase(IItemRepository itemRepository) : IGetItemUseCase
    {
        private readonly IItemRepository _itemRepository = itemRepository;

        public async Task<List<ItemDTO>> GetAllAsync()
        {
            var domainEntities = await _itemRepository.GetAllAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<ItemDTO> GetAll()
        {
            var domainEntities = _itemRepository.GetAll();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<ItemDTO?> GetAsync(int idItem)
        {
            var domainEntity = await _itemRepository.GetAsync(idItem);

            if (domainEntity == null)
            {
                Debug.WriteLine($"Item с ID {idItem} не найден в репозитории.");
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}