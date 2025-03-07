using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.TypeDeathCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.TypeDeathCase
{
    public class GetTypeDeathUseCase(ITypeDeathRepository typeDeathRepository) : IGetTypeDeathUseCase
    {
        private readonly ITypeDeathRepository _typeDeathRepository = typeDeathRepository;

        public async Task<List<TypeDeathDTO>> GetAllTypeDeathAsync()
        {
            var domainEntities = await _typeDeathRepository.GetAllAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<TypeDeathDTO> GetAll()
        {
            var domainEntities = _typeDeathRepository.GetAll();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<TypeDeathDTO?> GetTypeDeathAsync(int idTypeDeath)
        {
            var domainEntity = await _typeDeathRepository.GetAsync(idTypeDeath);

            if (domainEntity == null)
            {
                Debug.WriteLine($"TypeDeath с ID {idTypeDeath} не найден в репозитории.");
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}