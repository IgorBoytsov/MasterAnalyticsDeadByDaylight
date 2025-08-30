using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.OfferingCategory;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.OfferingCategories.Update
{
    public sealed class UpdateOfferingCategoryCommandHandler(
        IApplicationDbContext context,
        IOfferingCategoryRepository offeringCategoryRepository) : IRequestHandler<UpdateOfferingCategoryCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IOfferingCategoryRepository _offeringCategoryRepository = offeringCategoryRepository;

        public async Task<Result> Handle(UpdateOfferingCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var offeringCategory = await _offeringCategoryRepository.Get(request.OfferingCategoryId);

                if (offeringCategory is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данной записи не найдено."));

                offeringCategory.UpdateName(OfferingCategoryName.Create(request.Name));

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