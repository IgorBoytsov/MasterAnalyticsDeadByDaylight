using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.SurvivorPerkCategory;
using DBDAnalytics.Shared.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.Perks.AssignCategory
{
    public sealed class AssignCategoryToPerkCommandHandler(
        IApplicationDbContext context,
        ISurvivorRepository survivorRepository) : IRequestHandler<AssignCategoryToPerkCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly ISurvivorRepository _survivorRepository = survivorRepository;

        public async Task<Result> Handle(AssignCategoryToPerkCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var survivor = await _survivorRepository.GetSurvivor(request.SurvivorId);

                if (survivor is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данный выживший не найден"));

                var catId = SurvivorPerkCategoryId.From(request.CategoryId);

                survivor.AssignCategoryToPerk(catId, request.PerkId);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException ex)
            {
                return Result.Failure(ex.Error);
            }
            catch (Exception)
            {
                return Result.Failure(new Error(ErrorCode.Server, "Произошла непредвиденная ошибка при установки категории"));
            }
        }
    }
}
