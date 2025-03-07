using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.MeasurementCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Models;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.MeasurementCase
{
    public class GetMeasurementUseCase(IMeasurementRepository measurementRepository) : IGetMeasurementUseCase
    {
        private readonly IMeasurementRepository _measurementRepository = measurementRepository;

        public async Task<List<MeasurementDTO>> GetAllAsync()
        {
            var domainEntities = await _measurementRepository.GetAllAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<MeasurementDTO> GetAll()
        {
            var domainEntities = _measurementRepository.GetAll();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<MeasurementDTO?> GetAsync(int idMeasurement)
        {
            var domainEntity = await _measurementRepository.GetAsync(idMeasurement);

            if (domainEntity == null)
            {
                Debug.WriteLine($"Measurement с ID {idMeasurement} не найден в репозитории.");
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}