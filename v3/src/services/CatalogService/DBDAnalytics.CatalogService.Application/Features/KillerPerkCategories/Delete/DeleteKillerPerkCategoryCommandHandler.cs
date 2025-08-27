using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.KillerPerkCategories.Delete
{
    public sealed class DeleteKillerPerkCategoryCommandHandler(
        IApplicationDbContext context,
        IKillerPerkCategoryRepository killerPerkCategoryRepository) : IRequestHandler<DeleteKillerPerkCategoryCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IKillerPerkCategoryRepository _killerPerkCategoryRepository = killerPerkCategoryRepository;

        public async Task<Result> Handle(DeleteKillerPerkCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var kpc = await _killerPerkCategoryRepository.Get(request.Id);

                if (kpc is null)
                    return Result.Failure(new Error(ErrorCode.Delete, "Такой записи не существует."));

                _killerPerkCategoryRepository.Remove(kpc);

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