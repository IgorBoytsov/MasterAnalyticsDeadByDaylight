using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCategoryCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.SurvivorPerkCategoryCase
{
    public class UpdateSurvivorPerkCategoryUseCase(ISurvivorPerkCategoryRepository survivorPerkCategoryRepository) : IUpdateSurvivorPerkCategoryUseCase
    {
        private readonly ISurvivorPerkCategoryRepository _survivorCategoryRepository = survivorPerkCategoryRepository;

        public async Task<(SurvivorPerkCategoryDTO? SurvivorPerkCategoryDTO, string? Message)> UpdateAsync(int idSurvivorPerkCategory, string survivorPerkCategoryName)
        {
            string message = string.Empty;

            if (idSurvivorPerkCategory == 0 || string.IsNullOrWhiteSpace(survivorPerkCategoryName))
                return (null, "Такой записи не существует");

            var exist = await _survivorCategoryRepository.ExistAsync(survivorPerkCategoryName);

            if (exist)
                return (null, "Название на которое вы хотите поменять - уже существует.");

            int id = await _survivorCategoryRepository.UpdateAsync(idSurvivorPerkCategory, survivorPerkCategoryName);

            var domainEntity = await _survivorCategoryRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}
