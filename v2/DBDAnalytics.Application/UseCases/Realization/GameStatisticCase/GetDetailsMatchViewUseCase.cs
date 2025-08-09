using DBDAnalytics.Application.DTOs.DetailsViewDTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.GameStatisticCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.GameStatisticCase
{
    public class GetDetailsMatchViewUseCase(IDetailsMatchRepository detailsMatchRepository) : IGetDetailsMatchViewUseCase
    {
        private readonly IDetailsMatchRepository _detailsMatchRepository = detailsMatchRepository;
        public async Task<DetailsMatchViewDTO> GetDetailsViewMatch(int idGameStatistics)
        {
            var details = await _detailsMatchRepository.GetDetailsViewMatch(idGameStatistics);

            var dto = details.ToDTO();

            return dto;
        }
    }
}