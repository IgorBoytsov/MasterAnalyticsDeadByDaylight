using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Measurement;
using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.Update
{
    public sealed class UpdateMeasurementCommandHandler(
        IApplicationDbContext context,
        IMeasurementRepository measurementRepository) : IRequestHandler<UpdateMeasurementCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMeasurementRepository _measurementRepository = measurementRepository;

        public async Task<Result> Handle(UpdateMeasurementCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var measurement = await _measurementRepository.GetMeasurement(request.MeasurementId);

                if (measurement is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данной записи не найдено."));

                measurement.UpdateName(MeasurementName.Create(request.Name));

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(new Error(ErrorCode.Update, "Произошла непредвиденная ошибка на стороне сервера."));
            }
        }
    }
}