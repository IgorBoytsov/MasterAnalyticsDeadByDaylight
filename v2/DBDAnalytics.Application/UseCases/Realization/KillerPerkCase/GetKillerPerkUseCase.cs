using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.KillerPerkCase
{
    public class GetKillerPerkUseCase(IKillerPerkRepository killerPerkRepository) : IGetKillerPerkUseCase
    {
        private readonly IKillerPerkRepository _killerPerkRepository = killerPerkRepository;

        public async Task<List<KillerPerkDTO>> GetAllAsync()
        {
            var domainEntities = await _killerPerkRepository.GetAllAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<KillerPerkDTO> GetAll()
        {
            var domainEntities = _killerPerkRepository.GetAll();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<KillerPerkDTO?> GetAsync(int idPerk)
        {
            var domainEntity = await _killerPerkRepository.GetAsync(idPerk);

            if (domainEntity == null)
            {
                Debug.WriteLine($"illerPerk с ID {idPerk} не найден в репозитории.");
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}