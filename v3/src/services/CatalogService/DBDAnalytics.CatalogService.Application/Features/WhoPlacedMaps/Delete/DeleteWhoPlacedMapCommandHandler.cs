using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.WhoPlacedMaps.Delete
{
    public sealed class DeleteWhoPlacedMapCommandHandler(
        IApplicationDbContext context,
        IWhoPlacedMapRepository whoPlacedMapRepository) : IRequestHandler<DeleteWhoPlacedMapCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IWhoPlacedMapRepository _whoPlacedMapRepository = whoPlacedMapRepository;

        public async Task<Result> Handle(DeleteWhoPlacedMapCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var whoPlacedMap = await _whoPlacedMapRepository.Get(request.Id);

                if (whoPlacedMap is null)
                    return Result.Failure(new Error(ErrorCode.Delete, "Такой записи не существует."));

                _whoPlacedMapRepository.Remove(whoPlacedMap);

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