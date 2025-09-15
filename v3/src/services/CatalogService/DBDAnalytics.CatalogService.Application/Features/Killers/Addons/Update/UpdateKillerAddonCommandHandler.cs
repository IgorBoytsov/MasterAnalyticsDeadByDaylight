using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.KillerAddon;
using DBDAnalytics.Shared.Domain.Constants;
using DBDAnalytics.Shared.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Addons.Update
{
    public sealed class UpdateKillerAddonCommandHandler(
        IApplicationDbContext context,
        IKillerRepository killerRepository,
        IFileStorageService fileStorageService) : IRequestHandler<UpdateKillerAddonCommand, Result<string>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IKillerRepository _killerRepository = killerRepository;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<Result<string>> Handle(UpdateKillerAddonCommand request, CancellationToken cancellationToken)
        {
            var killer = await _killerRepository.GetKiller(request.KillerId);
            var fileCategory = FileStoragePaths.KillerAddons(killer.Name);
            ImageKey? newImageKey = null;

            if (killer is null)
                return Result<string>.Failure(new Error(ErrorCode.NotFound, "Запись с киллером не найдена."));

            try
            {
                var oldImageKey = killer.KillerAddons.FirstOrDefault(ka => ka.Id == request.AddonId)?.ImageKey;

                newImageKey = await _fileStorageService.UploadImage(request.Image!.Content, request.Image.FileName, request.Image.ContentType, fileCategory, request.SemanticImageName, cancellationToken);

                killer.UpdateAddon(request.AddonId, KillerAddonName.Create(request.Name), newImageKey);

                await _context.SaveChangesAsync(cancellationToken);

                if (oldImageKey is not null && oldImageKey != newImageKey)
                {
                    try
                    {
                        await _fileStorageService.DeleteImageAsync($"{fileCategory}/{oldImageKey.Value}", cancellationToken);
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
                    await _fileStorageService.DeleteImageAsync($"{fileCategory}/{newImageKey.Value}", cancellationToken);

                if (ex is DomainException domainEx)
                    return Result<string>.Failure(new Error(ErrorCode.Validation, domainEx.Message));

                return Result<string>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при обновлении записи."));
            }
        }
    }
}