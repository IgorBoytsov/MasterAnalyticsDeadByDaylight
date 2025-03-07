using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.MatchAttributeCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.MatchAttributeCase
{
    public class GetMatchAttributeUseCase(IMatchAttributeRepository matchAttributeRepository) : IGetMatchAttributeUseCase
    {
        private readonly IMatchAttributeRepository _matchAttributeRepository = matchAttributeRepository;

        public async Task<List<MatchAttributeDTO>> GetAllAsync(bool isHide)
        {
            var domainEntities = await _matchAttributeRepository.GetAllAsync(isHide);

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<MatchAttributeDTO> GetAll(bool isHide)
        {
            var domainEntities = _matchAttributeRepository.GetAll(isHide);

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<MatchAttributeDTO?> GetAsync(int idMatchAttribute)
        {
            var domainEntity = await _matchAttributeRepository.GetAsync(idMatchAttribute);

            if (domainEntity == null)
            {
                Debug.WriteLine($"MatchAttribute с ID {idMatchAttribute} не найден в репозитории.");
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}