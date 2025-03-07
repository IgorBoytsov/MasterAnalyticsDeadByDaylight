using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.KillerPerkCase
{
    public class CreateKillerPerkUseCase(IKillerPerkRepository killerPerkRepository) : ICreateKillerPerkUseCase
    {
        private readonly IKillerPerkRepository _killerPerkRepository = killerPerkRepository;

        public async Task<(KillerPerkDTO? KillerPerkDTO, string? Message)> CreateAsync(int idKiller, string perkName, byte[]? perkImage, int? idCategory, string? perkDescription)
        {
            string message = string.Empty;

            var (CreatedKillerPerk, Message) = KillerPerkDomain.Create(0, idKiller, perkName, perkImage, idCategory, perkDescription);

            if (CreatedKillerPerk is null)
            {
                return (null, Message);
            }

            var id = await _killerPerkRepository.CreateAsync(idKiller, perkName, perkImage, idCategory, perkDescription);

            var domainEntity = await _killerPerkRepository.GetAsync(id);

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}