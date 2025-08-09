using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.RarityCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.RarityCase
{
    public class UpdateRarityUseCase(IRarityRepository rarityRepository) : IUpdateRarityUseCase
    {
        private readonly IRarityRepository _rarityRepository = rarityRepository;

        public async Task<(RarityDTO? RarityDTO, string? Message)> UpdateAsync(int idRarity, string rarityName, string? description)
        {
            string message = string.Empty;

            if (idRarity == 0 || string.IsNullOrWhiteSpace(rarityName))
                return (null, "Такой записи не существует");

            var exist = await _rarityRepository.ExistAsync(rarityName);

            if (exist)
                return (null, "Название на которое вы хотите поменять - уже существует.");

            int id = await _rarityRepository.UpdateAsync(idRarity, rarityName, description);

            var domainEntity = await _rarityRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}