using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.StatisticCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.StatisticCase
{
    public class GetDetailsMatchUseCase(IDetailsMatchRepository killStatsRepository) : IGetDetailsMatchUseCase
    {
        private readonly IDetailsMatchRepository _killStatsRepository = killStatsRepository;

        public async Task<(List<DetailsMatchDTO> KillerDetails, int TotalMatches)> GetDetailsMatch(int idKiller, int idAssociation)
        {
            var (KillerDetails, TotalMatch) = await _killStatsRepository.GetDetailsMatch(idKiller, idAssociation);

            var dto = KillerDetails.Select(x => x.ToDTO());

            return (dto.ToList(), TotalMatch);
        }
    }
}