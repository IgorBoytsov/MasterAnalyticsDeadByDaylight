using AutoMapper;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.Shared.Contracts.Responses.Match;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.GameEvents.Create
{
    public sealed class CreateGameEventCommandHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IGameEventRepository gameEventRepository) : IRequestHandler<CreateGameEventCommand, Result<GameEventResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IGameEventRepository _gameEventRepository = gameEventRepository;
        public async Task<Result<GameEventResponse>> Handle(CreateGameEventCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var gameMode = GameEvent.Create(request.OldId, request.Name);

                await _gameEventRepository.AddAsync(gameMode, cancellationToken);
                
                await _context.SaveChangesAsync(cancellationToken);

                var dto = _mapper.Map<GameEventResponse>(gameMode);

                return Result<GameEventResponse>.Success(dto);
            }
            catch (DomainException ex)
            {
                return Result<GameEventResponse>.Failure(new Error(ErrorCode.Validation, ex.Message));
            }
            catch (Exception ex)
            {
                return Result<GameEventResponse>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи об игровом событие {ex.Message} {ex.InnerException?.Message}"));
            }
        }
    }
}