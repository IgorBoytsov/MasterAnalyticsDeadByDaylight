using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.SurvivorCase
{
    public class UpdateSurvivorUseCase(ISurvivorRepository survivorRepository) : IUpdateSurvivorUseCase
    {
        private readonly ISurvivorRepository _survivorRepository = survivorRepository;

        public async Task<(SurvivorDTO? SurvivorDTO, string? Message)> UpdateAsync(int idSurvivor, string survivorName, byte[]? survivorImage, string? survivorDescription)
        {
            string message = string.Empty;

            if (idSurvivor == 0 || string.IsNullOrWhiteSpace(survivorName))
                return (null, "Такой записи не существует");

            var exist = await _survivorRepository.ExistAsync(survivorName);

            if (exist)
                return (null, "Название на которое вы хотите поменять - уже существует.");

            int id = await _survivorRepository.UpdateAsync(idSurvivor, survivorName, survivorImage, survivorDescription);

            var domainEntity = await _survivorRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }

        public async Task<SurvivorDTO?> ForcedUpdateAsync(int idSurvivor, string survivorName, byte[]? survivorImage, string? survivorDescription)
        {
            int id = await _survivorRepository.UpdateAsync(idSurvivor, survivorName, survivorImage, survivorDescription);

            var domainEntity = await _survivorRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}