using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.MapCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.MapCase
{
    public class UpdateMapUseCase(IMapRepository mapRepository) : IUpdateMapUseCase
    {
        private readonly IMapRepository _mapRepository = mapRepository;

        public async Task<(MapDTO? MapDTO, string? Message)> UpdateAsync(int idMap, int idMeasurement, string mapName, byte[] mapImage, string mapDescription)
        {
            string message = string.Empty;

            if (idMap == 0 || string.IsNullOrWhiteSpace(mapName))
                return (null, "Такой записи не существует");

            var exist = await _mapRepository.ExistAsync(mapName);

            if (exist)
                return (null, "Название на которое вы хотите поменять - уже существует.");

            int id = await _mapRepository.UpdateAsync(idMap, idMeasurement, mapName, mapImage, mapDescription);

            var domainEntity = await _mapRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }

        public async Task<MapDTO?> ForcedUpdateAsync(int idMap, int idMeasurement, string mapName, byte[] mapImage, string mapDescription)
        {
            int id = await _mapRepository.UpdateAsync(idMap, idMeasurement, mapName, mapImage, mapDescription);

            var domainEntity = await _mapRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}