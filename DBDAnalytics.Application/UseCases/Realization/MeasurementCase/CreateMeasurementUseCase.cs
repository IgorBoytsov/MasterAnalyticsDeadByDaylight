using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.MeasurementCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.MeasurementCase
{
    public class CreateMeasurementUseCase(IMeasurementRepository measurementRepository) : ICreateMeasurementUseCase
    {
        private readonly IMeasurementRepository _measurementRepository = measurementRepository;

        public async Task<(MeasurementDTO? MeasurementDTO, string? Message)> CreateAsync(string measurementName, string measurementDescription)
        {
            string message = string.Empty;

            var (CreatedMeasurement, Message) = MeasurementDomain.Create(0, measurementName, measurementDescription);

            if (CreatedMeasurement is null)
            {
                return (null, Message);
            }

            bool exist = await _measurementRepository.ExistAsync(measurementName);

            if (exist)
                return (null, "Запись с таким названием уже существует.");

            var id = await _measurementRepository.CreateAsync(measurementName, measurementDescription);

            var domainEntity = await _measurementRepository.GetAsync(id);

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}