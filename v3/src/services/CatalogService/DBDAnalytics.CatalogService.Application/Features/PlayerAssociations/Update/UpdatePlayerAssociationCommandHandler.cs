using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.PlayerAssociation;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.PlayerAssociations.Update
{
    public sealed class UpdatePlayerAssociationCommandHandler(
        IApplicationDbContext context,
        IPlayerAssociationRepository playerAssociationRepository) : IRequestHandler<UpdatePlayerAssociationCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IPlayerAssociationRepository _playerAssociationRepository = playerAssociationRepository;

        public async Task<Result> Handle(UpdatePlayerAssociationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var playerAssociation = await _playerAssociationRepository.Get(request.PlayerAssociationID);

                if (playerAssociation is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данной записи не найдено."));

                playerAssociation.UpdateName(PlayerAssociationName.Create(request.Name));

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