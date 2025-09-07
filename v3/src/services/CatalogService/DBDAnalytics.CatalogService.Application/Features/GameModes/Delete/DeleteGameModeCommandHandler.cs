using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.GameModes.Delete
{
    public sealed class DeleteGameModeCommandHandler(
		IApplicationDbContext context,
		IGameModeRepository gameModeRepository) : IRequestHandler<DeleteGameModeCommand, Result>
    {
		private readonly IApplicationDbContext _context = context;
		private readonly IGameModeRepository _gameModeRepository = gameModeRepository;

        public async Task<Result> Handle(DeleteGameModeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var gameMode = await _gameModeRepository.Get(request.Id);

                if (gameMode == null)
                    return Result.Failure(new Error(ErrorCode.Delete, "Такой записи не существует."));

                _gameModeRepository.Remove(gameMode);

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