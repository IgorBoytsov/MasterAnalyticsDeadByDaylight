using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.KillerAddonCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.KillerAddonCase
{
    public class UpdateKillerAddonUseCase(IKillerAddonRepository killerAddonRepository) : IUpdateKillerAddonUseCase
    {
        private readonly IKillerAddonRepository _killerAddonRepository = killerAddonRepository;

        public async Task<(KillerAddonDTO? KillerAddonDTO, string? Message)> UpdateAsync(int idAddon, int idKiller, int? idRarity, string addonName, byte[]? addonImage, string? addonDescription)
        {
            string message = string.Empty;

            if (idKiller == 0)
                return (null, "Вы не выбрали к какому предмету относиться улучшение.");

            if (idKiller == 0 || string.IsNullOrWhiteSpace(addonName))
                return (null, "Такой записи не существует");

            int id = await _killerAddonRepository.UpdateAsync(idAddon, idKiller, idRarity, addonName, addonImage, addonDescription);

            var domainEntity = await _killerAddonRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}