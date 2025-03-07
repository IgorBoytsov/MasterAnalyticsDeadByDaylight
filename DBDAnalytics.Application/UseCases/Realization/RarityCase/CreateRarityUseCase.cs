using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.RarityCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.RarityCase
{
    public class CreateRarityUseCase(IRarityRepository rarityRepository) : ICreateRarityUseCase
    {
        private readonly IRarityRepository _rarityRepository = rarityRepository;

        public async Task<(RarityDTO? RarityDTO, string? Message)> CreateAsync(string rarityName)
        {
            string message = string.Empty;

            var (CreatedRarity, Message) = RarityDomain.Create(0, rarityName);

            if (CreatedRarity is null)
            {
                return (null, Message);
            }

            bool exist = await _rarityRepository.ExistAsync(rarityName);

            if (exist)
                return (null, "Запись с таким названием уже существует.");

            var id = await _rarityRepository.CreateAsync(rarityName);

            var domainEntity = await _rarityRepository.GetAsync(id);

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}