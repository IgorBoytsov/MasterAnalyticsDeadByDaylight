using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.Shared.Domain.Constants;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.Maps.Delete
{
    public sealed class DeleteMapCommandHandler(
        IApplicationDbContext context,
        IMeasurementRepository measurementRepository,
        IFileStorageService fileStorageService) : IRequestHandler<DeleteMapCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMeasurementRepository _measurementRepository = measurementRepository;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<Result> Handle(DeleteMapCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var measurement = await _measurementRepository.GetMeasurement(request.MeasurementId);

                if (measurement is null)
                    return Result.Failure(new Error(ErrorCode.Delete, "Такой записи не существует."));

                var mapImageKey = measurement.Maps.FirstOrDefault(ia => ia.Id == request.MapId)?.ImageKey;

                var wasRemoved = measurement.RemoveMap(request.MapId);

                if (!wasRemoved)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Карта для удаления не найдена у данного измерения."));

                if (mapImageKey is not null)
                    await _fileStorageService.DeleteImageAsync($"{FileStoragePaths.Maps}/{mapImageKey.Value}", cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(new Error(ErrorCode.Delete, $"Произошла непредвиденная ошибка на стороне сервера"));
            }
        }
    }
}