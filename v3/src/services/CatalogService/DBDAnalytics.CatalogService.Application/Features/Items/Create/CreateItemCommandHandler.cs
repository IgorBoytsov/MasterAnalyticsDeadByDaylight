using AutoMapper;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.Shared.Contracts.Constants;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Items.Create
{
    public sealed class CreateItemCommandHandler(
		IApplicationDbContext context,
		IItemRepository itemRepository,
		IMapper mapper,
		IFileUploadManager fileUploadManager) : IRequestHandler<CreateItemCommand, Result<ItemResponse>>
    {
		private readonly IApplicationDbContext _context = context;
		private readonly IItemRepository _itemRepository = itemRepository;
		private readonly IMapper _mapper = mapper;
		private readonly IFileUploadManager _fileUploadManager = fileUploadManager;

        public async Task<Result<ItemResponse>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
			try
			{
				ImageKey? imageKey = await _fileUploadManager.UploadImageAsync(request.Image, FileStoragePaths.Items, request.SemanticImageName, cancellationToken);
				var item = Item.Create(request.OldId, request.Name, imageKey);

				foreach (var itemAddon in request.Addons)
				{
					ImageKey? imageKeyAddon = await _fileUploadManager.UploadImageAsync(itemAddon.Image, FileStoragePaths.ItemAddons(item.Name), itemAddon.SemanticImageName, cancellationToken);
					item.AddAddon(itemAddon.OldId, itemAddon.Name, imageKeyAddon, itemAddon.RarityId);
				}

				await _itemRepository.AddAsync(item, cancellationToken);

				var dto = _mapper.Map<ItemResponse>(item);

				await _context.SaveChangesAsync(cancellationToken);

				return Result<ItemResponse>.Success(dto);

            }
            catch (Exception ex)
            {
                await _fileUploadManager.RollbackUploadsAsync(cancellationToken);

                if (ex is DomainException domainEx)
                    return Result<ItemResponse>.Failure(new Error(ErrorCode.Validation, domainEx.Message));

                return Result<ItemResponse>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи об предметах {ex.Message} {ex.InnerException?.Message}"));
            }
        }
    }
}