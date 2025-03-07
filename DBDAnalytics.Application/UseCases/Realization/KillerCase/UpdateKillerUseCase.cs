using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.KillerCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.KillerCase
{
    public class UpdateKillerUseCase(IKillerRepository killerRepository) : IUpdateKillerUseCase
    {
        private readonly IKillerRepository _killerRepository = killerRepository;

        public async Task<(KillerDTO? KillerDTO, string? Message)> UpdateAsync(int idKiller, string killerName, byte[]? killerImage, byte[]? killerAbilityImage)
        {
            string message = string.Empty;

            if (idKiller == 0 || string.IsNullOrWhiteSpace(killerName))
                return (null, "Такой записи не существует");

            var exist = await _killerRepository.ExistAsync(killerName);

            if (exist)
                return (null, "Название на которое вы хотите поменять - уже существует.");

            int id = await _killerRepository.UpdateAsync(idKiller, killerName, killerImage, killerAbilityImage);

            var domainEntity = await _killerRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }

        public async Task<KillerDTO?> ForcedUpdateAsync(int idKiller, string killerName, byte[]? killerImage, byte[]? killerAbilityImage)
        {
            int id = await _killerRepository.UpdateAsync(idKiller, killerName, killerImage, killerAbilityImage);

            var domainEntity = await _killerRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}