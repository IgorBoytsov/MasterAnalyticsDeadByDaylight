using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.StatisticCase;
using DBDAnalytics.Domain.Enums;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.StatisticCase
{
    public class GetDetailsMatchUseCase(IDetailsMatchRepository killStatsRepository) : IGetDetailsMatchUseCase
    {
        private readonly IDetailsMatchRepository _killStatsRepository = killStatsRepository;

        public async Task<(List<DetailsMatchDTO> KillerDetails, int TotalMatches)> GetDetailsMatch(int idEntity, Associations associations, FilterParameter filterParameter)
        {
            var (KillerDetails, TotalMatch) = await _killStatsRepository.GetDetailsMatch(idEntity, associations, filterParameter);

            var dto = KillerDetails.Select(x => x.ToDTO());

            return (dto.ToList(), TotalMatch);
        } 
        
        public async Task<(List<DetailsMatchDTO> KillerDetails, int TotalMatches)> GetDetailsMatch(List<int> idsMatches)
        {
            var (KillerDetails, TotalMatch) = await _killStatsRepository.GetDetailsMatch(idsMatches);

            var dto = KillerDetails.Select(x => x.ToDTO());

            return (dto.ToList(), TotalMatch);
        }
    }
}