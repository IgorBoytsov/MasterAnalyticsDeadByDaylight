using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.SurvivorCase
{
    public class CreateSurvivorUseCase(ISurvivorRepository survivorRepository) : ICreateSurvivorUseCase
    {
        private readonly ISurvivorRepository _survivorRepository = survivorRepository;

        public async Task<(SurvivorDTO? SurvivorDTO, string Message)> CreateAsync(string survivorName, byte[]? survivorImage, string? survivorDescription)
        {
            string message = string.Empty;

            var (CreatedSurvivor, Message) = SurvivorDomain.Create(0, survivorName, survivorImage, survivorDescription);

            if (CreatedSurvivor is null)
            {
                return (null, Message);
            }

            bool exist = await _survivorRepository.ExistAsync(survivorName);

            if (exist)
                return (null, "Запись с таким названием уже существует.");

            var id = await _survivorRepository.CreateAsync(survivorName, survivorImage, survivorDescription);

            var domainEntity = await _survivorRepository.GetAsync(id);

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}