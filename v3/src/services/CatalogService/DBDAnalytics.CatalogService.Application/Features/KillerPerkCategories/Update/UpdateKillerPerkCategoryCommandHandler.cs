using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Killer;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.KillerPerkCategories.Update
{
    public sealed class UpdateKillerPerkCategoryCommandHandler(
        IApplicationDbContext context,
        IKillerPerkCategoryRepository killerPerkCategoryRepository) : IRequestHandler<UpdateKillerPerkCategoryCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IKillerPerkCategoryRepository _killerPerkCategoryRepository = killerPerkCategoryRepository;

        public async Task<Result> Handle(UpdateKillerPerkCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var kpc = await _killerPerkCategoryRepository.Get(request.KillerPerkCategoryId);

                if (kpc is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данной записи не найдено."));

                kpc.UpdateName(KillerPerkCategoryName.Create(request.Name));

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(new Error(ErrorCode.Update, "Произошла непредвиденная ошибка на стороне сервера."));
            }
        }
    }
}