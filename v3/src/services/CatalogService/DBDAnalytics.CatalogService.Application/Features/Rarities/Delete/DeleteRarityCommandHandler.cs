using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Rarities.Delete
{
    public sealed class DeleteRarityCommandHandler(
        IApplicationDbContext context,
        IRarityRepository rarityRepository) : IRequestHandler<DeleteRarityCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IRarityRepository _rarityRepository = rarityRepository;

        public async Task<Result> Handle(DeleteRarityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var rarity = await _rarityRepository.Get(request.Id);

                if (rarity is null)
                    return Result.Failure(new Error(ErrorCode.Delete, "Такой записи не существует."));

                _rarityRepository.Remove(rarity);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(new Error(ErrorCode.Delete, "Произошла непредвиденная ошибка на стороне сервера."));
            }
        }
    }
}