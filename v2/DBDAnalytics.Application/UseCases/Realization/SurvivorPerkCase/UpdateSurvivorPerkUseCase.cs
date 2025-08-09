using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.SurvivorPerkCase
{
    public class UpdateSurvivorPerkUseCase(ISurvivorPerkRepository survivorPerkRepository) : IUpdateSurvivorPerkUseCase
    {
        private readonly ISurvivorPerkRepository _survivorPerkRepository = survivorPerkRepository;

        public async Task<(SurvivorPerkDTO? SurvivorPerkDTO, string? Message)> UpdateAsync(int idPerk, int idSurvivor, string perkName, byte[]? perkImage, int? idCategory, string? perkDescription)
        {
            string message = string.Empty;

            if (idPerk == 0 || string.IsNullOrWhiteSpace(perkName))
                return (null, "Такой записи не существует");

            int id = await _survivorPerkRepository.UpdateAsync(idPerk, idSurvivor, perkName, perkImage, idCategory, perkDescription);

            var domainEntity = await _survivorPerkRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}