using AutoMapper;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.Shared.Contracts.Responses.Match;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.GameModes.Create
{
    public sealed class CreateGameModeCommandHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IGameModeRepository gameModeRepository) : IRequestHandler<CreateGameModeCommand, Result<GameModeResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IGameModeRepository _gameModeRepository = gameModeRepository;

        public async Task<Result<GameModeResponse>> Handle(CreateGameModeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var gameMode = GameMode.Create(request.OldId, request.Name);

                await _gameModeRepository.AddAsync(gameMode, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                var dto = _mapper.Map<GameModeResponse>(gameMode);

                return Result<GameModeResponse>.Success(dto);
            }
            catch (DomainException ex)
            {
                return Result<GameModeResponse>.Failure(new Error(ErrorCode.Validation, ex.Message));
            }
            catch (Exception ex)
            {
                return Result<GameModeResponse>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи об игровом режиме {ex.Message} {ex.InnerException?.Message}"));
            }
        }
    }
}