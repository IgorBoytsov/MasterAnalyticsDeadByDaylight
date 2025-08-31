using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.Shared.Domain.Constants;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Perks.Delete
{
    public sealed class DeleteKillerPerkCommandHandler(
        IApplicationDbContext context,
        IKillerRepository killerRepository,
        IFileStorageService fileStorageService) : IRequestHandler<DeleteKillerPerkCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IKillerRepository _killerRepository = killerRepository;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<Result> Handle(DeleteKillerPerkCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var killer = await _killerRepository.GetKiller(request.KillerId);

                if (killer is null)
                    return Result.Failure(new Error(ErrorCode.Delete, "Такой записи не существует."));

                var itemImageKey = killer.KillerPerks.FirstOrDefault(ia => ia.Id == request.KillerPerkId)?.ImageKey;

                var wasRemoved = killer.RemovePerk(request.KillerPerkId);

                if (!wasRemoved)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Перка для удаления не найдено у данного киллера."));

                if (itemImageKey is not null)
                    await _fileStorageService.DeleteImageAsync($"{FileStoragePaths.KillerPerks(killer.Name)}/{itemImageKey.Value}", cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(new Error(ErrorCode.Delete, $"Произошла непредвиденная ошибка на стороне сервера"));
            }
        }
    }
}