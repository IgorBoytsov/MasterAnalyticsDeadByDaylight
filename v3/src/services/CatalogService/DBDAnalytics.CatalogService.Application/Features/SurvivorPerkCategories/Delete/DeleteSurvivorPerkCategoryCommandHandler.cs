using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.SurvivorPerkCategories.Delete
{
    public sealed class DeleteSurvivorPerkCategoryCommandHandler(
        IApplicationDbContext context,
        ISurvivorPerkCategoryRepository survivorPerkCategoryRepository) : IRequestHandler<DeleteSurvivorPerkCategoryCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly ISurvivorPerkCategoryRepository _survivorPerkCategoryRepository = survivorPerkCategoryRepository;

        public async Task<Result> Handle(DeleteSurvivorPerkCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var spc = await _survivorPerkCategoryRepository.Get(request.Id);

                if (spc is null)
                    return Result.Failure(new Error(ErrorCode.Delete, "Такой записи не существует."));

                _survivorPerkCategoryRepository.Remove(spc);

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