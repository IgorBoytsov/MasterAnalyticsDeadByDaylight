using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.SurvivorCase
{
    public class GetSurvivorWithPerksUseCase(ISurvivorRepository survivorRepository) : IGetSurvivorWithPerksUseCase
    {
        private readonly ISurvivorRepository _survivorRepository = survivorRepository;

        public async Task<List<SurvivorWithPerksDTO>> GetSurvivorsWithPerksAsync()
        {
            var domainEntities = await _survivorRepository.GetSurvivorsWithPerksAsync();

            var dtoEntities = domainEntities.ToSurvivorsWithPerksDTO();

            return dtoEntities;
        }

        public async Task<SurvivorWithPerksDTO> GetSurvivorWithPerksAsync(int idSurvivor)
        {
            var domainEntities = await _survivorRepository.GetSurvivorWithPerksAsync(idSurvivor);

            var dtoEntities = domainEntities.ToSurvivorWithPerksDTO();

            return dtoEntities;
        }
    }
}