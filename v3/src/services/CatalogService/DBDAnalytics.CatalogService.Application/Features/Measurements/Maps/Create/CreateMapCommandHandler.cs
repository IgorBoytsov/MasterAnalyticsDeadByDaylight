using AutoMapper;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.Shared.Contracts.Responses.Maps;
using DBDAnalytics.Shared.Domain.Constants;
using DBDAnalytics.Shared.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.Maps.Create
{
    public sealed class CreateMapCommandHandler(
        IApplicationDbContext context,
        IMeasurementRepository measurementRepository,
        IMapper mapper,
        IFileUploadManager fileUploadManager) : IRequestHandler<CreateMapCommand, Result<List<MapResponse>>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMeasurementRepository _measurementRepository = measurementRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IFileUploadManager _fileUploadManager = fileUploadManager;

        public async Task<Result<List<MapResponse>>> Handle(CreateMapCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<Map> addingMaps = [];
                List<AddMapToMeasurementCommandData> addMaps = [];

                var measurementId = request.Maps.FirstOrDefault()?.MeasurementId;

                if (measurementId == null)
                    return Result<List<MapResponse>>.Failure(new Error(ErrorCode.NotFound, "У измерения нету идентификатора."));

                #region Проверка на дупликаты записей

                var requestMapsNames = request.Maps
                    .Select(a => a.Name)
                    .ToList();

                var existingMapNames = await _context.Maps
                    .Where(map => map.MeasurementId == measurementId && requestMapsNames.Contains(map.Name))
                    .Select(map => map.Name)
                    .ToListAsync(cancellationToken);

                if (existingMapNames.Any())
                    return Result<List<MapResponse>>.Failure(new Error(ErrorCode.Validation, $"Следующие карты уже существуют для этого измерения: {string.Join(", ", existingMapNames)}"));

                #endregion

                var measurement = await _measurementRepository.GetMeasurement(measurementId.Value);

                if (measurement == null)
                    return Result<List<MapResponse>>.Failure(new Error(ErrorCode.NotFound, $"Измерение не найдено."));

                foreach (var map in request.Maps)
                {
                    ImageKey? imageKey = await _fileUploadManager.UploadImageAsync(map.Image, FileStoragePaths.Maps, map.SemanticImageName, cancellationToken);
                    var mapAdd = measurement.AddMap(map.OldId, map.Name, imageKey);
                    addingMaps.Add(mapAdd);
                }

                var dto = _mapper.Map<List<MapResponse>>(addingMaps);

                await _context.SaveChangesAsync(cancellationToken);

                return Result<List<MapResponse>>.Success(dto);
            }
            catch (Exception ex)
            {
                await _fileUploadManager.RollbackUploadsAsync(cancellationToken);

                if (ex is DomainException domainEx)
                    return Result<List<MapResponse>>.Failure(new Error(ErrorCode.Validation, domainEx.Message));

                return Result<List<MapResponse>>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи о карте {ex.Message} {ex.InnerException?.Message}"));
            }
        }
    }
}