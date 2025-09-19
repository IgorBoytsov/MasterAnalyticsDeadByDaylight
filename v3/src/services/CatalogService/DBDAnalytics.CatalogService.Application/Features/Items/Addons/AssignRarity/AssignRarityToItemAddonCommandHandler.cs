using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Items.Addons.AssignRarity
{
    public sealed class AssignRarityToItemAddonCommandHandler(
        IApplicationDbContext context,
        IItemRepository itemRepository) : IRequestHandler<AssignRarityToItemAddonCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IItemRepository _itemRepository = itemRepository;

        public async Task<Result> Handle(AssignRarityToItemAddonCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var item = await _itemRepository.GetItem(request.ItemId);

                if (item is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данный предмет не найден"));

                var rarity = await _context.Rarities.FirstOrDefaultAsync(c => c.Id == request.RarityId, cancellationToken);

                if (rarity is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данная редкость не найдена"));

                item.AssignRarity(request.ItemAddonId, rarity.Id);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException ex)
            {
                return Result.Failure(ex.Error);
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.Server, $"Произошла непредвиденная ошибка при установки редкости {ex.Message}"));
            }
        }
    }
}