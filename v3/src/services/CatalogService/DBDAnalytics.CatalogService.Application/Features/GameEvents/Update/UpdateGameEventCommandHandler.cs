using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.GameEvent;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.GameEvents.Update
{
    public sealed class UpdateGameEventCommandHandler(
        IApplicationDbContext context,
        IGameEventRepository gameEventRepository) : IRequestHandler<UpdateGameEventCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IGameEventRepository _gameEventRepository = gameEventRepository;

        public async Task<Result> Handle(UpdateGameEventCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var gameEvent = await _gameEventRepository.Get(request.GameEventId);

                if (gameEvent is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данной записи не найдено."));

                gameEvent.UpdateName(GameEventName.Create(request.Name));

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