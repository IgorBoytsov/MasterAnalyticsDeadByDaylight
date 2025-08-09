using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.MeasurementCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class MeasurementService(ICreateMeasurementUseCase createMeasurementUseCase,
                                    IDeleteMeasurementUseCase deleteMeasurementUseCase,
                                    IGetMeasurementUseCase getMeasurementUseCase,
                                    IGetMeasurementWithMapsUseCase getMeasurementWithMapsUseCase,
                                    IUpdateMeasurementUseCase updateMeasurementUseCase) : IMeasurementService
    {
        private readonly ICreateMeasurementUseCase _createMeasurementUseCase = createMeasurementUseCase;
        private readonly IDeleteMeasurementUseCase _deleteMeasurementUseCase = deleteMeasurementUseCase;
        private readonly IGetMeasurementUseCase _getMeasurementUseCase = getMeasurementUseCase;
        private readonly IGetMeasurementWithMapsUseCase _getMeasurementWithMapsUseCase = getMeasurementWithMapsUseCase;
        private readonly IUpdateMeasurementUseCase _updateMeasurementUseCase = updateMeasurementUseCase;

        public async Task<(MeasurementDTO? MeasurementDTO, string Message)> CreateAsync(string measurementName, string measurementDescription)
        {
            return await _createMeasurementUseCase.CreateAsync(measurementName, measurementDescription);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idMeasurement)
        {
            return await _deleteMeasurementUseCase.DeleteAsync(idMeasurement);
        }

        public List<MeasurementDTO> GetAll()
        {
            return _getMeasurementUseCase.GetAll();
        }

        public async Task<List<MeasurementDTO>> GetAllAsync()
        {
            return await _getMeasurementUseCase.GetAllAsync();
        }

        public async Task<MeasurementDTO> GetAsync(int idMeasurement)
        {
            return await _getMeasurementUseCase.GetAsync(idMeasurement);
        }

        public async Task<(MeasurementDTO? MeasurementDTO, string? Message)> UpdateAsync(int idMeasurement, string measurementName, string measurementDescription)
        {
            return await _updateMeasurementUseCase.UpdateAsync(idMeasurement, measurementName, measurementDescription);
        }

        public async Task<MeasurementDTO> ForcedUpdateAsync(int idMeasurement, string measurementName, string measurementDescription)
        {
            return await _updateMeasurementUseCase.ForcedUpdateAsync(idMeasurement, measurementName, measurementDescription);
        }

        public List<MeasurementWithMapsDTO?> GetMeasurementsWithMaps()
        {
            return _getMeasurementWithMapsUseCase.GetMeasurementsWithMaps();
        }

        public async Task<List<MeasurementWithMapsDTO?>> GetMeasurementsWithMapsAsync()
        {
            return await _getMeasurementWithMapsUseCase.GetMeasurementsWithMapsAsync();
        }

        public MeasurementWithMapsDTO? GetMeasurementWithMaps(int idMeasurement)
        {
            return _getMeasurementWithMapsUseCase.GetMeasurementWithMaps(idMeasurement);
        }

        public async Task<MeasurementWithMapsDTO?> GetMeasurementWithMapsAsync(int idMeasurement)
        {
            return await _getMeasurementWithMapsUseCase.GetMeasurementWithMapsAsync(idMeasurement);
        }
    }
}