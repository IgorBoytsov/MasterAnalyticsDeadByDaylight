using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.SurvivorPerkCase
{
    public class CreateSurvivorPerkUseCase(ISurvivorPerkRepository survivorPerkRepository) : ICreateSurvivorPerkUseCase
    {
        private readonly ISurvivorPerkRepository _survivorPerkRepository = survivorPerkRepository;

        public async Task<(SurvivorPerkDTO? SurvivorPerkDTO, string? Message)> CreateAsync(int idSurvivor, string perkName, byte[]? perkImage, int? idCategory, string? perkDescription)
        {
            string message = string.Empty;

            var (CreatedPerk, Message) = SurvivorPerkDomain.Create(0, idSurvivor, perkName, perkImage, idCategory, perkDescription);

            if (CreatedPerk is null)
            {
                return (null, Message);
            }

            var id = await _survivorPerkRepository.CreateAsync(idSurvivor, perkName, perkImage, idCategory, perkDescription);

            var domainEntity = await _survivorPerkRepository.GetAsync(id);

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}