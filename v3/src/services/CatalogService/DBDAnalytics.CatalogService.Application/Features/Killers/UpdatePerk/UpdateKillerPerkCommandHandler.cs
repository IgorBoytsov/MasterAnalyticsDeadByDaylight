using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Killer;
using DBDAnalytics.CatalogService.Domain.ValueObjects.KillerAddon;
using DBDAnalytics.Shared.Domain.Constants;
using DBDAnalytics.Shared.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.UpdatePerk
{
    public sealed class UpdateKillerPerkCommandHandler(
        IApplicationDbContext context,
        IKillerRepository killerRepository,
        IFileStorageService fileStorageService) : IRequestHandler<UpdateKillerPerkCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IKillerRepository _killerRepository = killerRepository;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<Result> Handle(UpdateKillerPerkCommand request, CancellationToken cancellationToken)
        {
            var killer = await _killerRepository.GetKiller(request.KillerId);
            var fileCategory = FileStoragePaths.KillerPerks(killer.Name);
            ImageKey? newImageKey = null;

            if (killer is null)
                return Result.Failure(new Error(ErrorCode.NotFound, "Запись с киллером не найдена."));

            try
            {
                var oldImageKey = killer.KillerAddons.FirstOrDefault(ka => ka.Id == request.PerkId)?.ImageKey;

                newImageKey = await _fileStorageService.UploadImage(request.Image!.Content, request.Image.FileName, request.Image.ContentType, fileCategory, request.SemanticImageName, cancellationToken);

                killer.UpdatePerk(request.PerkId, KillerPerkName.Create(request.Name), newImageKey);

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
