using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.KillerAddonCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.KillerAddonCase
{
    public class CreateKillerAddonUseCase(IKillerAddonRepository killerAddonRepository) : ICreateKillerAddonUseCase
    {
        private readonly IKillerAddonRepository _killerAddonRepository = killerAddonRepository;

        public async Task<(KillerAddonDTO? KillerAddonDTO, string? Message)> CreateAsync(int idKiller, int? idRarity, string addonName, byte[]? addonImage, string? addonDescription)
        {
            string message = string.Empty;

            var (CreatedKillerAddon, Message) = KillerAddonDomain.Create(0, idKiller, idRarity, addonName, addonImage, addonDescription);

            if (CreatedKillerAddon is null)
            {
                return (null, Message);
            }

            var id = await _killerAddonRepository.CreateAsync(idKiller, idRarity, addonName, addonImage, addonDescription);

            var domainEntity = await _killerAddonRepository.GetAsync(id);

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}