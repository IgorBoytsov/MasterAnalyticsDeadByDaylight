using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Killer;
using DBDAnalytics.Shared.Domain.Constants;
using DBDAnalytics.Shared.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Update
{
    public sealed class UpdateKillerCommandHandler(
        IApplicationDbContext context,
        IKillerRepository killerRepository,
        IFileStorageService fileStorageService) : IRequestHandler<UpdateKillerCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IKillerRepository _killerRepository = killerRepository;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<Result> Handle(UpdateKillerCommand request, CancellationToken cancellationToken)
        {
            var killer = await _killerRepository.GetKiller(request.Id);

            ImageKey? newImagePortraitKey = null;
            ImageKey? newImageAbilityKey = null;

            if (killer is null)
                return Result.Failure(new Error(ErrorCode.NotFound, "Запись не найдена."));

            try
            {
                var oldImagePortraitKey = killer.KillerImageKey;
                var oldImageAbilityKey = killer.AbilityImageKey;

                newImagePortraitKey = await _fileStorageService.UploadImage(request.ImagePortrait!.Content, request.ImagePortrait.FileName, request.ImagePortrait.ContentType, FileStoragePaths.KillerPortraits, request.SemanticImagePortraitName, cancellationToken);
                newImageAbilityKey = await _fileStorageService.UploadImage(request.ImageAbility!.Content, request.ImageAbility.FileName, request.ImageAbility.ContentType, FileStoragePaths.KillerAbilities, request.SemanticImageAbilityName, cancellationToken);

                killer.UpdateName(KillerName.Create(request.Name));
                killer.UpdateImagePortrait(newImagePortraitKey);
                killer.UpdateImageAbility(newImageAbilityKey);

                await _context.SaveChangesAsync(cancellationToken);

                if (oldImagePortraitKey is not null && oldImagePortraitKey != newImagePortraitKey)
                {
                    try
                    {
                        await _fileStorageService.DeleteImageAsync($"{FileStoragePaths.KillerPortraits}/{oldImagePortraitKey.Value}", cancellationToken);
                    }
                    catch (Exception)
                    {
                        // TODO: Залогировать как критическую ошибку
                    }
                }

                if (oldImageAbilityKey is not null && oldImageAbilityKey != newImageAbilityKey)
                {
                    try
                    {
                        await _fileStorageService.DeleteImageAsync($"{FileStoragePaths.KillerAbilities}/{oldImageAbilityKey.Value}", cancellationToken);
                    }
                    catch (Exception)
                    {
                        // TODO: Залогировать как критическую ошибку
                    }
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                if (newImagePortraitKey is not null)
                    await _fileStorageService.DeleteImageAsync($"{FileStoragePaths.KillerPortraits}/{newImagePortraitKey.Value}", cancellationToken);

                if (newImageAbilityKey is not null)
                    await _fileStorageService.DeleteImageAsync($"{FileStoragePaths.KillerAbilities}/{newImageAbilityKey.Value}", cancellationToken);

                if (ex is DomainException domainEx)
                    return Result.Failure(new Error(ErrorCode.Validation, domainEx.Message));

                return Result.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при обновлении записи."));
            }
        }
    }
}