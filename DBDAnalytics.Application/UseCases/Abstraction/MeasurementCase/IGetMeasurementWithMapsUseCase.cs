using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.MeasurementCase
{
    public interface IGetMeasurementWithMapsUseCase
    {
        List<MeasurementWithMapsDTO?> GetMeasurementsWithMaps();
        Task<List<MeasurementWithMapsDTO?>> GetMeasurementsWithMapsAsync();
        MeasurementWithMapsDTO? GetMeasurementWithMaps(int idMeasurement);
        Task<MeasurementWithMapsDTO?> GetMeasurementWithMapsAsync(int idMeasurement);
    }
}