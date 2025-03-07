using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.RarityCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.RarityCase
{
    public class GetRarityUseCase(IRarityRepository rarityRepository) : IGetRarityUseCase
    {
        private readonly IRarityRepository _rarityRepository = rarityRepository;

        public async Task<List<RarityDTO>> GetAllAsync()
        {
            var domainEntities = await _rarityRepository.GetAllAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<RarityDTO> GetAll()
        {
            var domainEntities = _rarityRepository.GetAll();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<RarityDTO?> GetAsync(int idRarity)
        {
            var domainEntity = await _rarityRepository.GetAsync(idRarity);

            if (domainEntity == null)
            {
                Debug.WriteLine($"Rarity с ID {idRarity} не найден в репозитории.");
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}
