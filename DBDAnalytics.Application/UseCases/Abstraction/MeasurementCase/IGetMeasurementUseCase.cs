using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.MeasurementCase
{
    public interface IGetMeasurementUseCase
    {
        List<MeasurementDTO> GetAll();
        Task<List<MeasurementDTO>> GetAllAsync();
        Task<MeasurementDTO?> GetAsync(int idMeasurement);
    }
}