using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCategoryCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.SurvivorPerkCategoryCase
{
    public class CreateSurvivorPerkCategoryUseCase(ISurvivorPerkCategoryRepository survivorPerkCategoryRepository) : ICreateSurvivorPerkCategoryUseCase
    {
        private readonly ISurvivorPerkCategoryRepository _survivorPerkCategoryRepository = survivorPerkCategoryRepository;

        public async Task<(SurvivorPerkCategoryDTO? SurvivorPerkCategoryDTO, string? Message)> CreateAsync(string survivorPerkCategoryName, string? description)
        {
            string message = string.Empty;

            var (CreationCategory, Message) = SurvivorPerkCategoryDomain.Create(0, survivorPerkCategoryName, description);

            if (CreationCategory is null)
            {
                return (null, Message);
            }

            bool exist = await _survivorPerkCategoryRepository.ExistAsync(survivorPerkCategoryName);

            if (exist)
                return (null, "Запись с таким названием уже существует.");

            var id = await _survivorPerkCategoryRepository.CreateAsync(survivorPerkCategoryName, description);

            var domainEntity = await _survivorPerkCategoryRepository.GetAsync(id);

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}