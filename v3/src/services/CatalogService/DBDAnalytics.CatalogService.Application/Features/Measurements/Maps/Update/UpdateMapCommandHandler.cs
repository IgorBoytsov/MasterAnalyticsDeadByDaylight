using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Map;
using DBDAnalytics.Shared.Contracts.Constants;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.Maps.Update
{
    public sealed class UpdateMapCommandHandler(
        IApplicationDbContext context,
        IMeasurementRepository measurementRepository,
        IFileStorageService fileStorageService) : IRequestHandler<UpdateMapCommand, Result<string>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMeasurementRepository _measurementRepository = measurementRepository;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<Result<string>> Handle(UpdateMapCommand request, CancellationToken cancellationToken)
        {
            var measurement = await _measurementRepository.GetMeasurement(request.MeasurementId);
            ImageKey? newImageKey = null;

            try
            {
                var map = measurement.Maps.FirstOrDefault(m => m.Id == request.MapId);

                if (map is null)
                    return Result<string>.Failure(new Error(ErrorCode.NotFound, "Запись не найдена."));

                var oldImageKey = map.ImageKey;

                newImageKey = await _fileStorageService.UploadImage(request.Image!.Content, request.Image.FileName, request.Image.ContentType, FileStoragePaths.Maps, request.SemanticImageName, cancellationToken);

                measurement.UpdateMap(request.MapId, MapName.Create(request.Name), newImageKey);

                await _context.SaveChangesAsync(cancellationToken);

                if (oldImageKey is not null && oldImageKey != newImageKey)
                {
                    try
                    {
                        await _fileStorageService.DeleteImageAsync($"{FileStoragePaths.Maps}/{oldImageKey.Value}", cancellationToken);
                    }
                    catch (Exception)
                    {
                        // TODO: Залогировать как критическую ошибку
                    }
                }

                return Result<string>.Success(newImageKey!);
            }
            catch (Exception ex)
            {
                if (newImageKey is not null)
                    await _fileStorageService.DeleteImageAsync($"{FileStoragePaths.Maps}/{newImageKey.Value}", cancellationToken);

                if (ex is DomainException domainEx)
                    return Result<string>.Failure(new Error(ErrorCode.Validation, domainEx.Message));

                return Result<string>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при обновлении записи."));
            }
        }
    }
}
