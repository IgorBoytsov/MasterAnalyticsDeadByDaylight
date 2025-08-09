using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.WhoPlacedMapCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.WhoPlacedMapCase
{
    public class UpdateWhoPlacedMapUseCase(IWhoPlacedMapRepository whoPlacedMapRepository) : IUpdateWhoPlacedMapUseCase
    {
        private readonly IWhoPlacedMapRepository _whoPlacedMapRepository = whoPlacedMapRepository;

        public async Task<(WhoPlacedMapDTO? WhoPlacedMapDTO, string? Message)> UpdateAsync(int idWhoPlacedMap, string whoPlacedMapName, string? description)
        {
            string message = string.Empty;

            if (idWhoPlacedMap == 0 || string.IsNullOrWhiteSpace(whoPlacedMapName))
                return (null, "Такой записи не существует");

            var exist = await _whoPlacedMapRepository.ExistAsync(whoPlacedMapName);

            if (exist)
                return (null, "Название на которое вы хотите поменять - уже существует.");

            int id = await _whoPlacedMapRepository.UpdateAsync(idWhoPlacedMap, whoPlacedMapName, description);

            var domainEntity = await _whoPlacedMapRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}