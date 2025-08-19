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

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.Create
{
    public sealed class CreateMeasurementCommandHandler(
        IApplicationDbContext context,
        IMeasurementRepository measurementRepository,
        IMapper mapper,
        IFileUploadManager fileUploadManager) : IRequestHandler<CreateMeasurementCommand, Result<MeasurementResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMeasurementRepository _measurementRepository = measurementRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IFileUploadManager _fileUploadManager = fileUploadManager;

        public async Task<Result<MeasurementResponse>> Handle(CreateMeasurementCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var measurement = Measurement.Create(request.OldId, request.Name);

                foreach (var item in request.Maps)
                {
                    ImageKey? imageKey = await _fileUploadManager.UploadImageAsync(item.Image, FileStoragePaths.Maps, item.SemanticImageName, cancellationToken);
                    measurement.AddMap(item.OldId, item.Name, imageKey);
                }

                await _measurementRepository.AddAsync(measurement, cancellationToken);

                var dto = _mapper.Map<MeasurementResponse>(measurement);

                await _context.SaveChangesAsync(cancellationToken);

                return Result<MeasurementResponse>.Success(dto);
            }
            catch (Exception ex)
            {
                await _fileUploadManager.RollbackUploadsAsync(cancellationToken);

                if (ex is DomainException domainEx)
                    return Result<MeasurementResponse>.Failure(new Error(ErrorCode.Validation, domainEx.Message));

                return Result<MeasurementResponse>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи об измерение {ex.Message} {ex.InnerException?.Message}"));
            }
        }
    }
}