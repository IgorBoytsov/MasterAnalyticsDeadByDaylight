using AutoMapper;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.Shared.Contracts.Responses.Killers;
using DBDAnalytics.Shared.Domain.Constants;
using DBDAnalytics.Shared.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.AddAddon
{
    public sealed class CreateKillerAddonCommandHandler(
        IApplicationDbContext context,
        IKillerRepository killerRepository,
        IMapper mapper,
        IFileUploadManager fileUploadManager) : IRequestHandler<CreateKillerAddonCommand, Result<List<KillerAddonResponse>>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IKillerRepository _killerRepository = killerRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IFileUploadManager _fileUploadManager = fileUploadManager;

        public async Task<Result<List<KillerAddonResponse>>> Handle(CreateKillerAddonCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var killerId = request.Addons.FirstOrDefault()?.KillerId;

                if (killerId == null)
                    return Result<List<KillerAddonResponse>>.Failure(new Error(ErrorCode.NotFound, "Киллер не найден, либо нечего добавлять."));

                var killer = await _killerRepository.GetKiller(killerId.Value);

                foreach (var addon in request.Addons)
                {
                    ImageKey? imageKey = await _fileUploadManager.UploadImageAsync(addon.Image, FileStoragePaths.KillerAddons(killer.Name), addon.SemanticImageName, cancellationToken);
                    killer.AddAddon(addon.OldId, addon.Name, imageKey);
                }

                var dto = _mapper.Map<List<KillerAddonResponse>>(killer.KillerAddons);

                await _context.SaveChangesAsync(cancellationToken);

                return Result<List<KillerAddonResponse>>.Success(dto);
            }
            catch (Exception ex)
            {
                await _fileUploadManager.RollbackUploadsAsync(cancellationToken);

                if (ex is DomainException domainEx)
                    return Result<List<KillerAddonResponse>>.Failure(new Error(ErrorCode.Validation, domainEx.Message));

                return Result<List<KillerAddonResponse>>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи об улучшение убийцы {ex.Message} {ex.InnerException?.Message}"));
            }
        }
    }
}