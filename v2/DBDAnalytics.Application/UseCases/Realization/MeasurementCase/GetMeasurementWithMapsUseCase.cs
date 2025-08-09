using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.MeasurementCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.MeasurementCase
{
    public class GetMeasurementWithMapsUseCase(IMeasurementRepository measurementRepository) : IGetMeasurementWithMapsUseCase
    {
        private readonly IMeasurementRepository _measurementRepository = measurementRepository;

        public async Task<List<MeasurementWithMapsDTO?>> GetMeasurementsWithMapsAsync()
        {
            var domainEntities = await _measurementRepository.GetMeasurementsWithMapsAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<MeasurementWithMapsDTO?> GetMeasurementsWithMaps()
        {
            var domainEntities = _measurementRepository.GetMeasurementsWithMaps();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<MeasurementWithMapsDTO?> GetMeasurementWithMapsAsync(int idMeasurement)
        {
            var domainEntity = await _measurementRepository.GetMeasurementWithMapsAsync(idMeasurement);

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }

        public MeasurementWithMapsDTO? GetMeasurementWithMaps(int idMeasurement)
        {
            var domainEntity = _measurementRepository.GetMeasurementWithMaps(idMeasurement);

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}