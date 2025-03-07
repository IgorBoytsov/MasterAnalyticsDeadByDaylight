using DBDAnalytics.Application.UseCases.Abstraction.SurvivorInfoCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.SurvivorInfoCase
{
    public class GetSurvivorInfoUseCase(ISurvivorInfoRepository survivorInfoRepository) : IGetSurvivorInfoUseCase
    {
        private readonly ISurvivorInfoRepository _survivorInfoRepository = survivorInfoRepository;

        public (List<int>? IdRecords, string? Message) GetLastNRecordsId(int countRecords)
        {
            string message = string.Empty;

            if (countRecords < 0)
                return (null, "Нельзя получить 0 или меньше записей.");

            var idRecords = _survivorInfoRepository.GetLastNRecordsId(countRecords);

            return (idRecords, message);
        }
    }
}