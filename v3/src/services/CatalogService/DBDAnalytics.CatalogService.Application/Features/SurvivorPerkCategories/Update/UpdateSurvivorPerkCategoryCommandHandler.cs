using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.SurvivorPerkCategory;
using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.SurvivorPerkCategories.Update
{
    public sealed class UpdateSurvivorPerkCategoryCommandHandler(
        IApplicationDbContext context,
        ISurvivorPerkCategoryRepository survivorPerkCategoryRepository) : IRequestHandler<UpdateSurvivorPerkCategoryCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly ISurvivorPerkCategoryRepository _survivorPerkCategoryRepository = survivorPerkCategoryRepository;

        public async Task<Result> Handle(UpdateSurvivorPerkCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var spc = await _survivorPerkCategoryRepository.Get(request.SurvivorPerkCategoryId);

                if (spc is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данной записи не найдено."));

                spc.UpdateName(SurvivorPerkCategoryName.Create(request.Name));

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