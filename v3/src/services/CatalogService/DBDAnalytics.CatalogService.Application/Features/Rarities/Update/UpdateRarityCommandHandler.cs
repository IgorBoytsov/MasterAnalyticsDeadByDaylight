using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Rarity;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Rarities.Update
{
    public sealed class UpdateRarityCommandHandler(
        IApplicationDbContext context,
        IRarityRepository rarityRepository) : IRequestHandler<UpdateRarityCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IRarityRepository _rarityRepository = rarityRepository;

        public async Task<Result> Handle(UpdateRarityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var rarity = await _rarityRepository.Get(request.RarityId);

                if (rarity is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данной записи не найдено."));

                rarity.UpdateName(RarityName.Create(request.Name));

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException ex)
            {
                return Result.Failure(ex.Error);
            }
            catch (Exception)
            {
                return Result.Failure(new Error(ErrorCode.Update, "Произошла непредвиденная ошибка на стороне сервера."));
            }
        }
    }
}
