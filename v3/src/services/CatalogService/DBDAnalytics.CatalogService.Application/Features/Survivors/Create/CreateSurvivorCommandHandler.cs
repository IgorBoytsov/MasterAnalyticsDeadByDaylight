using AutoMapper;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using DBDAnalytics.Shared.Domain.Constants;
using DBDAnalytics.Shared.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.Create
{
    public sealed class CreateSurvivorCommandHandler(
        IApplicationDbContext context,
        ISurvivorRepository survivorRepository,
        IMapper mapper,
        IFileUploadManager fileUploadManager) : IRequestHandler<CreateSurvivorCommand, Result<SurvivorResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly ISurvivorRepository _survivorRepository = survivorRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IFileUploadManager _fileUploadManager = fileUploadManager;

        public async Task<Result<SurvivorResponse>> Handle(CreateSurvivorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ImageKey? survivorImageKey = await _fileUploadManager.UploadImageAsync(request.Image, FileStoragePaths.SurvivorPortraits, request.SemanticImageName, cancellationToken);
                var survivor = Survivor.Create(request.OldId, request.Name, survivorImageKey);

                foreach (var item in request.Perks)
                {
                    ImageKey? perkImageKey = await _fileUploadManager.UploadImageAsync(item.Image, FileStoragePaths.SurvivorPerks(survivor.Name), item.SemanticImageName, cancellationToken);
                    survivor.AddPerk(item.Name, item.OldId, perkImageKey, null);
                }

                await _survivorRepository.AddAsync(survivor, cancellationToken);

                var dto = _mapper.Map<SurvivorResponse>(survivor);

                await _context.SaveChangesAsync(cancellationToken);

                return Result<SurvivorResponse>.Success(dto);
            }
            catch (Exception ex)
            {
                await _fileUploadManager.RollbackUploadsAsync(cancellationToken);

                if (ex is DomainException domainEx)
                    return Result<SurvivorResponse>.Failure(new Error(ErrorCode.Validation, domainEx.Message));

                return Result<SurvivorResponse>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи об выжившем {ex.Message} {ex.InnerException?.Message}"));
            }
        }
    }
}