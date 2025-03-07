using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.MeasurementCase
{
    public interface IUpdateMeasurementUseCase
    {
        Task<(MeasurementDTO? MeasurementDTO, string? Message)> UpdateAsync(int idMeasurement, string measurementName, string measurementDescription);
        Task<MeasurementDTO?> ForcedUpdateAsync(int idMeasurement, string measurementName, string measurementDescription);
    }
}