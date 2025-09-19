using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.Shared.Contracts.Constants;
using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Items.Delete
{
    public sealed class DeleteItemCommandHandler(
        IApplicationDbContext context,
        IItemRepository itemRepository,
        IFileStorageService fileStorageService) : IRequestHandler<DeleteItemCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IItemRepository _itemRepository = itemRepository;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<Result> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var itemAddonImageKeys = new List<ImageKey?>();

                var item = await _itemRepository.GetItem(request.Id);

                if (item is null)
                    return Result.Failure(new Error(ErrorCode.Delete, "Такой записи не существует."));

                var itemName = item.Name;
                var itemImageKey = item.ImageKey;

                foreach (var itemAddon in item.ItemAddons)
                    itemAddonImageKeys.Add(itemAddon.ImageKey);

                _itemRepository.Remove(item);

                await _fileStorageService.DeleteImageAsync($"{FileStoragePaths.Items}/{itemImageKey}", cancellationToken);

                foreach (var key in itemAddonImageKeys)
                    await _fileStorageService.DeleteImageAsync($"{FileStoragePaths.Items}/{itemName}/{key}", cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(new Error(ErrorCode.Delete, $"Произошла непредвиденная ошибка на стороне сервера."));
            }
        }
    }
}