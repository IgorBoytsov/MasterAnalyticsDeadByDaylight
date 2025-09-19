using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.OfferingCategories.Delete
{
    public sealed class DeleteOfferingCategoryCommandHandler(
        IApplicationDbContext context,
        IOfferingCategoryRepository offeringCategoryRepository) : IRequestHandler<DeleteOfferingCategoryCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IOfferingCategoryRepository _offeringCategoryRepository = offeringCategoryRepository;

        public async Task<Result> Handle(DeleteOfferingCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var offeringCategory = await _offeringCategoryRepository.Get(request.Id);

                if (offeringCategory is null)
                    return Result.Failure(new Error(ErrorCode.Delete, "Такой записи не существует."));

                _offeringCategoryRepository.Remove(offeringCategory);

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