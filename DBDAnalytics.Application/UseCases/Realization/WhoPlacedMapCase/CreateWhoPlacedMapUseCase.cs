using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.WhoPlacedMapCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.WhoPlacedMapCase
{
    public class CreateWhoPlacedMapUseCase(IWhoPlacedMapRepository whoPlacedMapRepository) : ICreateWhoPlacedMapUseCase
    {
        private readonly IWhoPlacedMapRepository _whoPlacedMapRepository = whoPlacedMapRepository;

        public async Task<(WhoPlacedMapDTO? WhoPlacedMapDTO, string? Message)> CreateAsync(string whoPlacedMapName)
        {
            string message = string.Empty;

            var (CreatedWhoPlaceMap, Message) = WhoPlacedMapDomain.Create(0, whoPlacedMapName);

            if (message is null)
            {
                return (null, Message);
            }

            bool exist = await _whoPlacedMapRepository.ExistAsync(whoPlacedMapName);

            if (exist)
                return (null, "Запись с таким названием уже существует.");

            var id = await _whoPlacedMapRepository.CreateAsync(whoPlacedMapName);

            var domainEntity = await _whoPlacedMapRepository.GetAsync(id);

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}