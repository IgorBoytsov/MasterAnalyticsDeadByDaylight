using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.Shared.Contracts.Constants;
using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.Delete
{
    public sealed class DeleteMeasurementCommandHandler(
        IApplicationDbContext context,
        IMeasurementRepository measurementRepository,
        IFileStorageService fileStorageService) : IRequestHandler<DeleteMeasurementCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMeasurementRepository _measurementRepository = measurementRepository;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<Result> Handle(DeleteMeasurementCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var mapImageKeys = new List<ImageKey?>();

                var measurement = await _measurementRepository.GetMeasurement(request.Id);

                if (measurement is null)
                    return Result.Failure(new Error(ErrorCode.Delete, "Такой записи не существует."));

                var measurementName = measurement.Name;

                foreach (var map in measurement.Maps)
                    mapImageKeys.Add(map.ImageKey);

                _measurementRepository.Remove(measurement);

                foreach (var key in mapImageKeys)
                    await _fileStorageService.DeleteImageAsync($"{FileStoragePaths.Maps}/{key}", cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(new Error(ErrorCode.Delete, $"Произошла непредвиденная ошибка на стороне сервера."));
            }
        }
    }
}