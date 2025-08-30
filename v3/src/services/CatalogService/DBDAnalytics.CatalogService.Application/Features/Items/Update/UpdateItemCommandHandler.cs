using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Item;
using DBDAnalytics.Shared.Domain.Constants;
using DBDAnalytics.Shared.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Items.Update
{
    public sealed class UpdateItemCommandHandler(
        IApplicationDbContext context,
        IItemRepository itemRepository,
        IFileStorageService fileStorageService) : IRequestHandler<UpdateItemCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IItemRepository _itemRepository = itemRepository;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<Result> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetItem(request.Id);
            var fileCategory = FileStoragePaths.Items;
            ImageKey? newImageKey = null;

            if (item is null)
                return Result.Failure(new Error(ErrorCode.NotFound, "Запись не найдена."));

            try
            {
                var oldImageKey = item.ImageKey;

                newImageKey =  await _fileStorageService.UploadImage(request.Image!.Content, request.Image.FileName, request.Image.ContentType, fileCategory, request.SemanticImageName, cancellationToken);

                item.UpdateName(ItemName.Create(request.Name));
                item.UpdateImageKey(newImageKey);

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

                return Result.Failure(new Error(ErrorCode.Create, "Произошла непредвиденная ошибка при обновлении записи."));
            }
        }
    }
}