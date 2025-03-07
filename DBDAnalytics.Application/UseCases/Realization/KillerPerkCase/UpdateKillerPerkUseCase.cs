using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.KillerPerkCase
{
    public class UpdateKillerPerkUseCase(IKillerPerkRepository killerPerkRepository) : IUpdateKillerPerkUseCase
    {
        private readonly IKillerPerkRepository _killerPerkRepository = killerPerkRepository;

        public async Task<(KillerPerkDTO? KillerPerkDTO, string? Message)> UpdateAsync(int idPerk, int idKiller, string perkName, byte[]? perkImage, int? idCategory, string? perkDescription)
        {
            string message = string.Empty;

            if (idPerk == 0 || string.IsNullOrWhiteSpace(perkName))
                return (null, "Такой записи не существует");

            int id = await _killerPerkRepository.UpdateAsync(idPerk, idKiller, perkName, perkImage, idCategory, perkDescription);

            var domainEntity = await _killerPerkRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}