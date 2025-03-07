using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.PlatformCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.PlatformCase
{
    public class CreatePlatformUseCase(IPlatformRepository platformRepository) : ICreatePlatformUseCase
    {
        private readonly IPlatformRepository _platformRepository = platformRepository;

        public async Task<(PlatformDTO? PlatformDTO, string? Message)> CreateAsync(string platformName, string platformDescription)
        {
            string message = string.Empty;

            var (CreatedPlatform, Message) = PlatformDomain.Create(0, platformName, platformDescription);

            if (CreatedPlatform is null)
            {
                return (null, Message);
            }

            bool exist = await _platformRepository.ExistAsync(platformName);

            if (exist)
                return (null, "Запись с таким названием уже существует.");

            var id = await _platformRepository.CreateAsync(platformName, platformDescription);

            var domainEntity = await _platformRepository.GetAsync(id);

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}