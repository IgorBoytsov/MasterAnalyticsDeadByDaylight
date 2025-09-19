using AutoMapper;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.Shared.Contracts.Constants;
using DBDAnalytics.Shared.Contracts.Responses.Killers;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Create
{
    public sealed class CreateKillerCommandHandler(
        IApplicationDbContext context,
        IKillerRepository killerRepository,
        IMapper mapper,
        IFileUploadManager fileUploadManager) : IRequestHandler<CreateKillerCommand, Result<KillerResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IKillerRepository _killerRepository = killerRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IFileUploadManager _fileUploadManager = fileUploadManager;
    
        public async Task<Result<KillerResponse>> Handle(CreateKillerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ImageKey? killerImageKey = await _fileUploadManager.UploadImageAsync(request.KillerImage, FileStoragePaths.KillerPortraits, request.SemanticKillerImageName, cancellationToken);
                ImageKey? abilityImageKey = await _fileUploadManager.UploadImageAsync(request.AbilityImage, FileStoragePaths.KillerAbilities, request.SemanticAbilityImageName, cancellationToken);

                var killer = Killer.Create(request.OldId, request.Name, killerImageKey, abilityImageKey);

                foreach (var item in request.Perks)
                {
                    ImageKey? imageKey = await _fileUploadManager.UploadImageAsync(item.Image, FileStoragePaths.KillerPerks(killer.Name), item.SemanticImageName, cancellationToken);
                    killer.AddPerk(item.OldId, item.Name, imageKey, null);
                }

                foreach (var item in request.Addons)
                {
                    ImageKey? imageKey = await _fileUploadManager.UploadImageAsync(item.Image, FileStoragePaths.KillerAddons(killer.Name), item.SemanticImageName, cancellationToken);
                    killer.AddAddon(item.OldId, item.Name, imageKey);
                }

                await _killerRepository.AddAsync(killer, cancellationToken);

                var dto = _mapper.Map<KillerResponse>(killer);

                await _context.SaveChangesAsync(cancellationToken);

                return Result<KillerResponse>.Success(dto);
            }
            catch (Exception ex)
            {
                await _fileUploadManager.RollbackUploadsAsync(cancellationToken);

                if (ex is DomainException domainEx)
                    return Result<KillerResponse>.Failure(new Error(ErrorCode.Validation, domainEx.Message));

                return Result<KillerResponse>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи об убийцы {ex.Message} {ex.InnerException?.Message}"));
            }
        }
    }
}