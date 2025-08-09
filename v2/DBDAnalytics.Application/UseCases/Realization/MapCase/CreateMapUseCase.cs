using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.MapCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.MapCase
{
    public class CreateMapUseCase(IMapRepository mapRepository) : ICreateMapUseCase
    {
        private readonly IMapRepository _mapRepository = mapRepository;

        public async Task<(MapDTO? MapDTO, string? Message)> CreateAsync(int idMeasurement, string mapName, byte[] mapImage, string mapDescription)
        {
            string message = string.Empty;

            var (CreatedMap, Message) = MapDomain.Create(0, mapName, mapImage, mapDescription, idMeasurement);

            if (CreatedMap is null)
            {
                return (null, Message);
            }

            bool exist = await _mapRepository.ExistAsync(mapName);

            if (exist)
                return (null, "Запись с таким названием уже существует.");

            var id = await _mapRepository.CreateAsync(idMeasurement, mapName, mapImage, mapDescription);

            var domainEntity = await _mapRepository.GetAsync(id);

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}