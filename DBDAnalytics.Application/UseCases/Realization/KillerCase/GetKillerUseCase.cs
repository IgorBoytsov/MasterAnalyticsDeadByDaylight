using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.KillerCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.KillerCase
{
    public class GetKillerUseCase(IKillerRepository killerRepository) : IGetKillerUseCase
    {
        private readonly IKillerRepository _killerRepository = killerRepository;

        public async Task<List<KillerDTO>> GetAllAsync()
        {
            var domainEntities = await _killerRepository.GetAllAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<KillerDTO> GetAll()
        {
            var domainEntities = _killerRepository.GetAll();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<KillerDTO?> GetAsync(int idKiller)
        {
            var domainEntity = await _killerRepository.GetAsync(idKiller);

            if (domainEntity == null)
            {
                Debug.WriteLine($"Killer с ID {idKiller} не найден в репозитории.");
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}