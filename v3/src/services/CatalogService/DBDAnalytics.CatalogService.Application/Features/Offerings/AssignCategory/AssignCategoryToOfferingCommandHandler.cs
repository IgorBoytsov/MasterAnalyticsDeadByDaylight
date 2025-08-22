using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.AssignCategory
{
    public sealed class AssignCategoryToOfferingCommandHandler(IApplicationDbContext context) : IRequestHandler<AssignCategoryToOfferingCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<Result> Handle(AssignCategoryToOfferingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var offerings = await _context.Offerings.FirstOrDefaultAsync(k => k.Id == request.OfferingId, cancellationToken);

                if (offerings is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данное подношение не найдено"));

                var cat = await _context.OfferingCategories.FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);

                if (cat is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данная категория не найдена"));

                offerings.AssignCategory(cat);

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