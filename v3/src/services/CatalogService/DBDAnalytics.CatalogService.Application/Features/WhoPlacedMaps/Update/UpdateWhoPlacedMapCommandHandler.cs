using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.WhoPlacedMap;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.WhoPlacedMaps.Update
{
    public sealed class UpdateWhoPlacedMapCommandHandler(
        IApplicationDbContext context,
        IWhoPlacedMapRepository whoPlacedMapRepository) : IRequestHandler<UpdateWhoPlacedMapCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IWhoPlacedMapRepository _whoPlacedMapRepository = whoPlacedMapRepository;

        public async Task<Result> Handle(UpdateWhoPlacedMapCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var wpm = await _whoPlacedMapRepository.Get(request.WhoPlacedMapId);

                if (wpm is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данной записи не найдено."));

                wpm.UpdateName(PlacedMapName.Create(request.Name));

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException ex)
            {
                return Result.Failure(ex.Error);
            }
            catch (Exception)
            {
                return Result.Failure(new Error(ErrorCode.Update, "Произошла непредвиденная ошибка на стороне сервера."));
            }
        }
    }
}