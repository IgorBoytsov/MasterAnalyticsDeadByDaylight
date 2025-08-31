using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.Shared.Domain.Constants;
using DBDAnalytics.Shared.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.Perks.Update
{
    public sealed class UpdateSurvivorPerkCommandHandler(
        IApplicationDbContext context,
        ISurvivorRepository survivorRepository,
        IFileStorageService fileStorageService) : IRequestHandler<UpdateSurvivorPerkCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly ISurvivorRepository _survivorRepository = survivorRepository;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<Result> Handle(UpdateSurvivorPerkCommand request, CancellationToken cancellationToken)
        {
            var survivor = await _survivorRepository.GetSurvivor(request.SurvivorId);
            var fileCategory = FileStoragePaths.SurvivorPerks(survivor.Name);
            ImageKey? newImageKey = null;

            try
            {
                var itemAddonToUpdate = survivor.SurvivorPerks.FirstOrDefault(sp => sp.Id == request.PerkId);

                if (itemAddonToUpdate is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Запись не найдена."));

                var oldImageKey = itemAddonToUpdate.ImageKey;

                newImageKey = await _fileStorageService.UploadImage(request.Image!.Content, request.Image.FileName, request.Image.ContentType, fileCategory, request.SemanticImageName, cancellationToken);

                survivor.UpdatePerk(request.PerkId, request.Name, newImageKey);

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

                return Result.Success();
            }
            catch (Exception ex)
            {
                if (newImageKey is not null)
                    await _fileStorageService.DeleteImageAsync($"{fileCategory}/{newImageKey.Value}", cancellationToken);

                if (ex is DomainException domainEx)
                    return Result.Failure(new Error(ErrorCode.Validation, domainEx.Message));

                return Result.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при обновлении записи."));
            }
        }
    }
}