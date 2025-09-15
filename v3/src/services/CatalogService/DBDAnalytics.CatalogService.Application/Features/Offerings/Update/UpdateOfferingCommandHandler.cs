using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Offering;
using DBDAnalytics.CatalogService.Domain.ValueObjects.OfferingCategory;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Rarity;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Role;
using DBDAnalytics.Shared.Domain.Constants;
using DBDAnalytics.Shared.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using System.Data;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.Update
{
    public sealed class UpdateOfferingCommandHandler(
        IApplicationDbContext context,
        IOfferingRepository offeringRepository,
        IFileStorageService fileStorageService) : IRequestHandler<UpdateOfferingCommand, Result<string>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IOfferingRepository _offeringRepository = offeringRepository;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<Result<string>> Handle(UpdateOfferingCommand request, CancellationToken cancellationToken)
        {
            var offering = await _offeringRepository.Get(request.Id);
            ImageKey? newImageKey = null;
            ImageKey? oldImageKey = null;
            string newStoragePath = string.Empty;
            string oldStoragePath = string.Empty;

            if (offering is null)
                return Result<string>.Failure(new Error(ErrorCode.NotFound, "Запись не найдена."));

            try
            {
                if (request.Image is not null)
                {
                    if (request.RoleId == (int)Shared.Domain.Enums.Roles.Survivor)
                        newStoragePath = FileStoragePaths.OfferingSurvivor;
                    if (request.RoleId == (int)Shared.Domain.Enums.Roles.Killer)
                        newStoragePath = FileStoragePaths.OfferingKiller;
                    if (request.RoleId == (int)Shared.Domain.Enums.Roles.General)
                        newStoragePath = FileStoragePaths.OfferingGeneral;

                    if (offering.RoleId == (int)Shared.Domain.Enums.Roles.Survivor)
                        oldStoragePath = FileStoragePaths.OfferingSurvivor;
                    if (offering.RoleId == (int)Shared.Domain.Enums.Roles.Killer)
                        oldStoragePath = FileStoragePaths.OfferingKiller;
                    if (offering.RoleId == (int)Shared.Domain.Enums.Roles.General)
                        oldStoragePath = FileStoragePaths.OfferingGeneral;

                    oldImageKey = offering.ImageKey;

                    newImageKey = await _fileStorageService.UploadImage(request.Image!.Content, request.Image.FileName, request.Image.ContentType, newStoragePath, request.SemanticImageName, cancellationToken);
                }

                if (newImageKey is not null && newImageKey != oldImageKey)
                    offering.UpdateImageKey(newImageKey);

                offering.UpdateName(OfferingName.Create(request.Name));
                offering.AssignRole(RoleId.Form(request.RoleId));

                if (request.CategoryId.HasValue)
                    offering.AssignCategory(OfferingCategoryId.From(request.CategoryId.Value));

                if (request.RarityId.HasValue)
                    offering.AssignRarity(RarityId.From(request.RarityId.Value));

                await _context.SaveChangesAsync(cancellationToken);

                if (oldImageKey is not null && oldImageKey != newImageKey)
                {
                    try
                    {
                        await _fileStorageService.DeleteImageAsync($"{oldStoragePath}/{oldImageKey.Value}", cancellationToken);
                    }
                    catch (Exception)
                    {
                        // TODO: Залогировать как критическую ошибку
                    }
                }

                if (newImageKey is not null && newImageKey != oldImageKey)
                    return Result<string>.Success(newImageKey!);
                else
                    return Result<string>.Success(offering.ImageKey!);


            }
            catch (Exception ex)
            {
                if (newImageKey is not null)
                    await _fileStorageService.DeleteImageAsync($"{newStoragePath}/{newImageKey.Value}", cancellationToken);

                if (ex is DomainException domainEx)
                    return Result<string>.Failure(new Error(ErrorCode.Validation, domainEx.Message));

                return Result<string>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при обновлении записи."));
            }
        }
    }
}