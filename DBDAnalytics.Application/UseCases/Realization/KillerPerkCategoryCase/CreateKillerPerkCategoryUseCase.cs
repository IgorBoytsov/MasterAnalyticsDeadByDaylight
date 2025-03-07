using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCategoryCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.KillerPerkCategoryCase
{
    public class CreateKillerPerkCategoryUseCase(IKillerPerkCategoryRepository KillerPerkCategoryRepository) : ICreateKillerPerkCategoryUseCase
    {
        private readonly IKillerPerkCategoryRepository _KillerPerkCategoryRepository = KillerPerkCategoryRepository;

        public async Task<(KillerPerkCategoryDTO? KillerPerkCategoryDTO, string? Message)> CreateAsync(string KillerPerkCategoryName)
        {
            string message = string.Empty;

            var (CreatedKillerPerkCategory, Message) = KillerPerkCategoryDomain.Create(0, KillerPerkCategoryName);

            if (CreatedKillerPerkCategory is null)
            {
                return (null, Message);
            }

            bool exist = await _KillerPerkCategoryRepository.ExistAsync(KillerPerkCategoryName);

            if (exist)
                return (null, "Запись с таким названием уже существует.");

            var id = await _KillerPerkCategoryRepository.CreateAsync(KillerPerkCategoryName);

            var domainEntity = await _KillerPerkCategoryRepository.GetAsync(id);

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}