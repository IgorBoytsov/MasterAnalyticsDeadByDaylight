using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.MeasurementCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.MeasurementCase
{
    public class UpdateMeasurementUseCase(IMeasurementRepository measurementRepository) : IUpdateMeasurementUseCase
    {
        private readonly IMeasurementRepository _measurementRepository = measurementRepository;

        public async Task<(MeasurementDTO? MeasurementDTO, string? Message)> UpdateAsync(int idMeasurement, string measurementName, string measurementDescription)
        {
            string message = string.Empty;

            if (idMeasurement == 0 || string.IsNullOrWhiteSpace(measurementName))
                return (null, "Такой записи не существует");

            var exist = await _measurementRepository.ExistAsync(measurementName);

            if (exist)
                return (null, "Название на которое вы хотите поменять - уже существует.");

            int id = await _measurementRepository.UpdateAsync(idMeasurement, measurementName, measurementDescription);

            var domainEntity = await _measurementRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }

        public async Task<MeasurementDTO?> ForcedUpdateAsync(int idMeasurement, string measurementName, string measurementDescription)
        {
            int id = await _measurementRepository.UpdateAsync(idMeasurement, measurementName, measurementDescription);

            var domainEntity = await _measurementRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}
