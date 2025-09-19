using AutoMapper;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.Shared.Contracts.Constants;
using DBDAnalytics.Shared.Contracts.Responses.Offering;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.Create
{
    public sealed class CreateOfferingCommandHandler(
        IApplicationDbContext context,
        IOfferingRepository offeringRepository,
        IFileUploadManager fileUploadManager,
        IMapper mapper) : IRequestHandler<CreateOfferingCommand, Result<OfferingResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IOfferingRepository _offeringRepository = offeringRepository;
        private readonly IFileUploadManager _fileUploadManager = fileUploadManager;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<OfferingResponse>> Handle(CreateOfferingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string filePath = FileStoragePaths.GetOfferingPathForRole(request.RoleId);

                ImageKey? imageKey = await _fileUploadManager.UploadImageAsync(request.Image, filePath, request.SemanticImageName, cancellationToken);
                var offering = Offering.Create(request.OldId, request.Name, imageKey, request.RoleId, request.RarityId, request.CategoryId);

                await _offeringRepository.AddAsync(offering, cancellationToken);

                var dto = _mapper.Map<OfferingResponse>(offering);

                await _context.SaveChangesAsync(cancellationToken);

                return Result<OfferingResponse>.Success(dto);
            }
            catch (Exception ex)
            {
                await _fileUploadManager.RollbackUploadsAsync(cancellationToken);

                if (ex is DomainException domainEx)
                    return Result<OfferingResponse>.Failure(new Error(ErrorCode.Validation, domainEx.Message));

                return Result<OfferingResponse>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи об подношение {ex.Message} {ex.InnerException?.Message}"));
            }
        }
    }
}