using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Offering;
using DBDAnalytics.Shared.Domain.Constants;
using DBDAnalytics.Shared.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.Update
{
    public sealed class UpdateOfferingCommandHandler(
        IApplicationDbContext context,
        IOfferingRepository offeringRepository,
        IFileStorageService fileStorageService) : IRequestHandler<UpdateOfferingCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IOfferingRepository _offeringRepository = offeringRepository;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<Result> Handle(UpdateOfferingCommand request, CancellationToken cancellationToken)
        {
            var offering = await _offeringRepository.Get(request.Id);
            ImageKey? newImageKey = null;
            string storagePath = string.Empty;

            if (offering is null)
                return Result.Failure(new Error(ErrorCode.NotFound, "Запись не найдена."));

            try
            {
                if (offering.RoleId == (int)Shared.Domain.Enums.Roles.Survivor)
                    storagePath = FileStoragePaths.OfferingSurvivor;
                if (offering.RoleId == (int)Shared.Domain.Enums.Roles.Killer)
                    storagePath = FileStoragePaths.OfferingKiller;
                if (offering.RoleId == (int)Shared.Domain.Enums.Roles.General)
                    storagePath = FileStoragePaths.OfferingGeneral;

                var oldImageKey = offering.ImageKey;

                newImageKey = await _fileStorageService.UploadImage(request.Image!.Content, request.Image.FileName, request.Image.ContentType, storagePath, request.SemanticImageName, cancellationToken);

                offering.UpdateName(OfferingName.Create(request.Name));
                offering.UpdateImageKey(newImageKey);

                await _context.SaveChangesAsync(cancellationToken);

                if (oldImageKey is not null && oldImageKey != newImageKey)
                {
                    try
                    {
                        await _fileStorageService.DeleteImageAsync($"{storagePath}/{oldImageKey.Value}", cancellationToken);
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
                    await _fileStorageService.DeleteImageAsync($"{storagePath}/{newImageKey.Value}", cancellationToken);

                if (ex is DomainException domainEx)
                    return Result.Failure(new Error(ErrorCode.Validation, domainEx.Message));

                return Result.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при обновлении записи."));
            }
        }
    }
}