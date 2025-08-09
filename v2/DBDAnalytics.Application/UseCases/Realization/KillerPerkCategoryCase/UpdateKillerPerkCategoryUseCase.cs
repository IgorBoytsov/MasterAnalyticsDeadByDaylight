using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCategoryCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.KillerPerkCategoryCase
{
    public class UpdateKillerPerkCategoryUseCase(IKillerPerkCategoryRepository KillerPerkCategoryRepository) : IUpdateKillerPerkCategoryUseCase
    {
        private readonly IKillerPerkCategoryRepository _KillerPerkCategoryRepository = KillerPerkCategoryRepository;

        public async Task<(KillerPerkCategoryDTO? KillerPerkCategoryDTO, string? Message)> UpdateAsync(int idKillerPerkCategory, string KillerPerkCategoryName, string? description)
        {
            string message = string.Empty;

            if (idKillerPerkCategory == 0 || string.IsNullOrWhiteSpace(KillerPerkCategoryName))
                return (null, "Такой записи не существует");

            var exist = await _KillerPerkCategoryRepository.ExistAsync(KillerPerkCategoryName);

            if (exist)
                return (null, "Название на которое вы хотите поменять - уже существует.");

            int id = await _KillerPerkCategoryRepository.UpdateAsync(idKillerPerkCategory, KillerPerkCategoryName, description);

            var domainEntity = await _KillerPerkCategoryRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}