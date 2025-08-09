using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.KillerCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.KillerCase
{
    public class GetKillerLoadoutUseCase(IKillerRepository killerRepository) : IGetKillerLoadoutUseCase
    {
        private readonly IKillerRepository _killerRepository = killerRepository;

        public async Task<List<KillerLoadoutDTO>> GetKillersWithAddonsAndPerksAsync()
        {
            var domainEntities = await _killerRepository.GetKillersWithAddonsAndPerksAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<KillerLoadoutDTO> GetKillerWithAddonsAndPerksAsync(int idKiller)
        {
            var domainEntities = await _killerRepository.GetKillerWithAddonsAndPerksAsync(idKiller);

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }
    }
}