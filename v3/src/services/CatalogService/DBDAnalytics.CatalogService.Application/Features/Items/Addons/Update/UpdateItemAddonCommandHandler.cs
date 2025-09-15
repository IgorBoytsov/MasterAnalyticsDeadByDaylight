using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.Shared.Domain.Constants;
using DBDAnalytics.Shared.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Items.Addons.Update
{
    public sealed class UpdateItemAddonCommandHandler(
        IApplicationDbContext context,
        IItemRepository itemRepository,
        IFileStorageService fileStorageService) : IRequestHandler<UpdateItemAddonCommand, Result<string>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IItemRepository _itemRepository = itemRepository;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<Result<string>> Handle(UpdateItemAddonCommand request, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetItem(request.Id);
            var fileCategory = FileStoragePaths.ItemAddons(item.Name);
            ImageKey? newImageKey = null;

            try
            {
                var itemAddonToUpdate = item.ItemAddons.FirstOrDefault(ia => ia.Id == request.ItemAddonId);

                if (itemAddonToUpdate is null)
                    return Result<string>.Failure(new Error(ErrorCode.NotFound, "Запись не найдена."));

                var oldImageKey = itemAddonToUpdate.ImageKey;

                newImageKey = await _fileStorageService.UploadImage(request.Image!.Content, request.Image.FileName, request.Image.ContentType, fileCategory, request.SemanticImageName, cancellationToken);

                item.UpdateItemAddon(request.ItemAddonId, request.Name, newImageKey);

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
                        
                        //_logger.LogCritical(ex, "Не удалось удалить старый файл после обновления аддона {AddonId}.", $"{fileCategory}/{oldImageKey.Value}", request.ItemAddonId);
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