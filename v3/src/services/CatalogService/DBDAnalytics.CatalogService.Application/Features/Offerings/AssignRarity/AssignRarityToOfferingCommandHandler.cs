using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.AssignRarity
{
    public sealed class AssignRarityToOfferingCommandHandler(IApplicationDbContext context) : IRequestHandler<AssignRarityToOfferingCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<Result> Handle(AssignRarityToOfferingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var offerings = await _context.Offerings.FirstOrDefaultAsync(k => k.Id == request.OfferingId, cancellationToken);

                if (offerings is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данное подношение не найдено"));

                var rarity = await _context.Rarities.FirstOrDefaultAsync(c => c.Id == request.RarityId, cancellationToken);

                if (rarity is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данная редкость не найдена"));

                offerings.AssignRarity(rarity);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException ex)
            {
                return Result.Failure(ex.Error);
            }
            catch (Exception)
            {
                return Result.Failure(new Error(ErrorCode.Server, "Произошла непредвиденная ошибка при установки редкости"));
            }
        }
    }
}