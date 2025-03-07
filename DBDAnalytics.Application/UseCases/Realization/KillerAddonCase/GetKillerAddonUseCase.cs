using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.KillerAddonCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.KillerAddonCase
{
    public class GetKillerAddonUseCase(IKillerAddonRepository killerAddonRepository) : IGetKillerAddonUseCase
    {
        private readonly IKillerAddonRepository _killerAddonRepository = killerAddonRepository;

        public async Task<List<KillerAddonDTO>> GetAllAsync()
        {
            var domainEntities = await _killerAddonRepository.GetAllAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<KillerAddonDTO> GetAll()
        {
            var domainEntities = _killerAddonRepository.GetAll();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<KillerAddonDTO?> GetAsync(int idKillerAddon)
        {
            var domainEntity = await _killerAddonRepository.GetAsync(idKillerAddon);

            if (domainEntity == null)
            {
                Debug.WriteLine($"KillerAddon с ID {idKillerAddon} не найден в репозитории.");
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}