namespace DBDAnalytics.Application.UseCases.Abstraction.MeasurementCase
{
    public interface IDeleteMeasurementUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idMeasurement);
    }
}