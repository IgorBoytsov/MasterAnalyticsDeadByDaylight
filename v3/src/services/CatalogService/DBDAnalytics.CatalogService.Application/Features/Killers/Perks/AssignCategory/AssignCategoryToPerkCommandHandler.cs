using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Domain.ValueObjects.KillerPerkCategory;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Perks.AssignCategory
{
    public sealed class AssignCategoryToPerkCommandHandler(IApplicationDbContext context) : IRequestHandler<AssignCategoryToPerkCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<Result> Handle(AssignCategoryToPerkCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var killer = await _context.Killers.Include(p => p.KillerPerks).FirstOrDefaultAsync(k => k.Id == request.KillerId, cancellationToken);

                if (killer is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данный киллер не найден"));

                var catId = KillerPerkCategoryId.From(request.CategoryId);

                killer.AssignCategoryToPerk(request.PerkId, catId);

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