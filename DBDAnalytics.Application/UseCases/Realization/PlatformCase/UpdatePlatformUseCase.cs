using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.PlatformCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.PlatformCase
{
    public class UpdatePlatformUseCase(IPlatformRepository platformRepository) : IUpdatePlatformUseCase
    {
        private readonly IPlatformRepository _platformRepository = platformRepository;

        public async Task<(PlatformDTO? PlatformDTO, string? Message)> UpdateAsync(int idPlatform, string platformName, string platformDescription)
        {
            string message = string.Empty;

            if (idPlatform == 0 || string.IsNullOrWhiteSpace(platformName))
                return (null, "Такой записи не существует");

            var exist = await _platformRepository.ExistAsync(platformName);

            if (exist)
                return (null, "Название на которое вы хотите поменять - уже существует.");

            int id = await _platformRepository.UpdateAsync(idPlatform, platformName, platformDescription);

            var domainEntity = await _platformRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }

        public async Task<PlatformDTO?> ForcedUpdateAsync(int idPlatform, string platformName, string platformDescription)
        {
            int id = await _platformRepository.UpdateAsync(idPlatform, platformName, platformDescription);

            var domainEntity = await _platformRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}