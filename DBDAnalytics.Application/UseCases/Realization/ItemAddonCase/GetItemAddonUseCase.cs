using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.ItemAddonCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.ItemAddonCase
{
    public class GetItemAddonUseCase(IItemAddonRepository itemAddonRepository) : IGetItemAddonUseCase
    {
        private readonly IItemAddonRepository _itemAddonRepository = itemAddonRepository;

        public async Task<List<ItemAddonDTO>> GetAllAsync()
        {
            var domainEntities = await _itemAddonRepository.GetAllAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<ItemAddonDTO> GetAll()
        {
            var domainEntities = _itemAddonRepository.GetAll();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<ItemAddonDTO?> GetAsync(int idItemAddon)
        {
            var domainEntity = await _itemAddonRepository.GetAsync(idItemAddon);

            if (domainEntity == null)
            {
                Debug.WriteLine($"ItemAddon с ID {idItemAddon} не найден в репозитории.");
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}