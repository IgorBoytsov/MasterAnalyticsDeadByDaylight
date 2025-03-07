using DBDAnalytics.Application.UseCases.Abstraction.MeasurementCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.MeasurementCase
{
    public class DeleteMeasurementUseCase(IMeasurementRepository measurementRepository) : IDeleteMeasurementUseCase
    {
        private readonly IMeasurementRepository _measurementRepository = measurementRepository;

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idMeasurement)
        {
            string message = string.Empty;

            var existBeforeDelete = await _measurementRepository.ExistAsync(idMeasurement);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _measurementRepository.DeleteAsync(idMeasurement);

            var existAfterDelete = await _measurementRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}
