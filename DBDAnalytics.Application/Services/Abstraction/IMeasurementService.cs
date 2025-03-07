using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface IMeasurementService
    {
        Task<(MeasurementDTO? MeasurementDTO, string Message)> CreateAsync(string measurementName, string measurementDescription);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idMeasurement);
        List<MeasurementDTO> GetAll();
        Task<List<MeasurementDTO>> GetAllAsync();
        Task<MeasurementDTO> GetAsync(int idMeasurement);
        Task<(MeasurementDTO? MeasurementDTO, string? Message)> UpdateAsync(int idMeasurement, string measurementName, string measurementDescription);
        Task<MeasurementDTO> ForcedUpdateAsync(int idMeasurement, string measurementName, string measurementDescription);
        List<MeasurementWithMapsDTO?> GetMeasurementsWithMaps();
        Task<List<MeasurementWithMapsDTO?>> GetMeasurementsWithMapsAsync();
        MeasurementWithMapsDTO? GetMeasurementWithMaps(int idMeasurement);
        Task<MeasurementWithMapsDTO?> GetMeasurementWithMapsAsync(int idMeasurement);
    }
}