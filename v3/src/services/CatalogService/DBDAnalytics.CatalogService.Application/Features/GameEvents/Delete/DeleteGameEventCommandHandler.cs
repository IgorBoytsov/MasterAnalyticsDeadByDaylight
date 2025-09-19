using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.GameEvents.Delete
{
    public sealed class DeleteGameEventCommandHandler(
        IApplicationDbContext context,
        IGameEventRepository gameEventRepository) : IRequestHandler<DeleteGameEventCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IGameEventRepository _gameEventRepository = gameEventRepository;

        public async Task<Result> Handle(DeleteGameEventCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var gameEvent = await _gameEventRepository.Get(request.Id);

                if (gameEvent is null)
                    return Result.Failure(new Error(ErrorCode.Delete, "Такой записи не существует."));

                _gameEventRepository.Remove(gameEvent);

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
