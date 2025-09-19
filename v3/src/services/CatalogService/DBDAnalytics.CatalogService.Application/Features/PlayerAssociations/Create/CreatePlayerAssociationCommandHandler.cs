using AutoMapper;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.PlayerAssociations.Create
{
    public sealed class CreatePlayerAssociationCommandHandler(
        IApplicationDbContext context,
        IPlayerAssociationRepository playerAssociationRepository,
        IMapper mapper) : IRequestHandler<CreatePlayerAssociationCommand, Result<PlayerAssociationResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IPlayerAssociationRepository _playerAssociationRepository = playerAssociationRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<PlayerAssociationResponse>> Handle(CreatePlayerAssociationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var typeDeath = PlayerAssociation.Create(request.OldId, request.Name);

                await _playerAssociationRepository.AddAsync(typeDeath, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                var dto = _mapper.Map<PlayerAssociationResponse>(typeDeath);

                return Result<PlayerAssociationResponse>.Success(dto);
            }
            catch (DomainException ex)
            {
                return Result<PlayerAssociationResponse>.Failure(new Error(ErrorCode.Validation, ex.Message));
            }
            catch (Exception ex)
            {
                return Result<PlayerAssociationResponse>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи об игровой ассоциации {ex.Message} {ex.InnerException?.Message}"));
            }
        }
    }
}