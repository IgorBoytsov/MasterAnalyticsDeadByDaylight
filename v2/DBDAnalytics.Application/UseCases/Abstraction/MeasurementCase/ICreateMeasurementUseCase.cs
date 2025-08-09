using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.MeasurementCase
{
    public interface ICreateMeasurementUseCase
    {
        Task<(MeasurementDTO? MeasurementDTO, string? Message)> CreateAsync(string measurementName, string measurementDescription);
    }
}