using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.GameMode;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.GameModes.Update
{
    public sealed class UpdateGameModeCommandHandler(
        IApplicationDbContext context,
        IGameModeRepository gameModeRepository) : IRequestHandler<UpdateGameModeCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IGameModeRepository _gameModeRepository = gameModeRepository;

        public async Task<Result> Handle(UpdateGameModeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var gameMode = await _gameModeRepository.Get(request.GameModeId);

                if (gameMode is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данной записи не найдено."));

                gameMode.UpdateName(GameModeName.Create(request.Name));

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