using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.KillerCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.KillerCase
{
    public class CreateKillerUseCase(IKillerRepository killerRepository) : ICreateKillerUseCase
    {
        private readonly IKillerRepository _killerRepository = killerRepository;

        public async Task<(KillerDTO? KillerDTO, string? Message)> CreateAsync(string killerName, byte[]? killerImage, byte[]? killerAbilityImage)
        {
            string message = string.Empty;

            var (CreatedKiller, Message) = KillerDomain.Create(0, killerName, killerImage, killerAbilityImage);

            if (CreatedKiller is null)
            {
                return (null, Message);
            }

            bool exist = await _killerRepository.ExistAsync(killerName);

            if (exist)
                return (null, "Запись с таким названием уже существует.");

            var id = await _killerRepository.CreateAsync(killerName, killerImage, killerAbilityImage);

            var domainEntity = await _killerRepository.GetAsync(id);

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}