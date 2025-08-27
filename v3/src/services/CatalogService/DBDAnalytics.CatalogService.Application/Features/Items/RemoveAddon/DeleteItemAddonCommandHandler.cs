using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.Shared.Domain.Constants;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Items.RemoveAddon
{
    public sealed class DeleteItemAddonCommandHandler(
        IApplicationDbContext context,
        IItemRepository itemRepository,
        IFileStorageService fileStorageService) : IRequestHandler<DeleteItemAddonCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IItemRepository _itemRepository = itemRepository;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<Result> Handle(DeleteItemAddonCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var item = await _itemRepository.GetItem(request.IdItem);

                if (item is null)
                    return Result.Failure(new Error(ErrorCode.Delete, "Такой записи не существует."));

                var itemImageKey = item.ItemAddons.FirstOrDefault(ia => ia.Id == request.IdAddon)?.ImageKey;

                var wasRemoved = item.RemoveAddon(request.IdAddon);

                if (!wasRemoved)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Улучшение для удаления не найдено у данного предмета."));

                if (itemImageKey is not null)
                    await _fileStorageService.DeleteImageAsync($"{FileStoragePaths.Items}/{item.Name}/{itemImageKey.Value}", cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(new Error(ErrorCode.Delete, $"Произошла непредвиденная ошибка на стороне сервера"));
            }
        }
    }
}