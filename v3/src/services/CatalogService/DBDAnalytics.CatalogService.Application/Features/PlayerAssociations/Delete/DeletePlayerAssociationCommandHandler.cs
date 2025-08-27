using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.PlayerAssociations.Delete
{
    public sealed class DeletePlayerAssociationCommandHandler(
        IApplicationDbContext context,
        IPlayerAssociationRepository playerAssociationRepository) : IRequestHandler<DeletePlayerAssociationCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IPlayerAssociationRepository _playerAssociationRepository = playerAssociationRepository;

        public async Task<Result> Handle(DeletePlayerAssociationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var playerAssociation = await _playerAssociationRepository.Get(request.Id);

                if (playerAssociation is null)
                    return Result.Failure(new Error(ErrorCode.Delete, "Такой записи не существует."));

                _playerAssociationRepository.Remove(playerAssociation);

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