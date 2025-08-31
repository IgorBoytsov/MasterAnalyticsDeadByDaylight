using AutoMapper;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using DBDAnalytics.Shared.Domain.Constants;
using DBDAnalytics.Shared.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Items.Addons.Create
{
    public sealed class CreateItemAddonCommandHandler(
        IApplicationDbContext context,
        IItemRepository itemRepository,
        IMapper mapper,
        IFileUploadManager fileUploadManager) : IRequestHandler<CreateItemAddonCommand, Result<List<ItemAddonResponse>>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IItemRepository _itemRepository = itemRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IFileUploadManager _fileUploadManager = fileUploadManager;

        public async Task<Result<List<ItemAddonResponse>>> Handle(CreateItemAddonCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<ItemAddon> itemAddons = [];

                var ItemId = request.Addons.FirstOrDefault()?.ItemId;

                if (ItemId == null)
                    return Result<List<ItemAddonResponse>>.Failure(new Error(ErrorCode.NotFound, "У предмета нету идентификатора."));

                #region Проверка на дубликаты записей

                var requestAddonsNames = request.Addons
                    .Select(a => a.Name)
                    .ToList();

                var existingPerkNames = await _context.ItemsAddon
                    .Where(addon => addon.ItemId == ItemId && requestAddonsNames.Contains(addon.Name))
                    .Select(addon => addon.Name.Value)
                    .ToListAsync(cancellationToken);

                if (existingPerkNames.Any())
                    return Result<List<ItemAddonResponse>>.Failure(new Error(ErrorCode.Validation, $"Следующие перки уже существуют для этого предмета: {string.Join(", ", existingPerkNames)}"));

                #endregion

                var item = await _itemRepository.GetItem(ItemId.Value);

                if (item == null)
                    return Result<List<ItemAddonResponse>>.Failure(new Error(ErrorCode.NotFound, "Предмет не найден."));

                foreach (var itemAddon in request.Addons)
                {
                    ImageKey? imageKey = await _fileUploadManager.UploadImageAsync(itemAddon.Image, FileStoragePaths.ItemAddons(item.Name), itemAddon.SemanticImageName, cancellationToken);
                    var addPerk = item.AddAddon(itemAddon.OldId, itemAddon.Name, imageKey, itemAddon.RarityId);
                    itemAddons.Add(addPerk);
                }

                var dto = _mapper.Map<List<ItemAddonResponse>>(itemAddons);

                await _context.SaveChangesAsync(cancellationToken);

                return Result<List<ItemAddonResponse>>.Success(dto);
            }
            catch (Exception ex)
            {
                await _fileUploadManager.RollbackUploadsAsync(cancellationToken);

                if (ex is DomainException domainEx)
                    return Result<List<ItemAddonResponse>>.Failure(new Error(ErrorCode.Validation, domainEx.Message));

                return Result<List<ItemAddonResponse>>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи об улучшение предмета {ex.Message} {ex.InnerException?.Message}"));
            }
        }
    }
}