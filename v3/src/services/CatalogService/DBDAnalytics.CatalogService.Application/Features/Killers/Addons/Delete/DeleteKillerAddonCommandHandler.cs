using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.Shared.Contracts.Constants;
using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Addons.Delete
{
    public sealed class DeleteKillerAddonCommandHandler(
        IApplicationDbContext context,
        IKillerRepository killerRepository,
        IFileStorageService fileStorageService) : IRequestHandler<DeleteKillerAddonCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IKillerRepository _killerRepository = killerRepository;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<Result> Handle(DeleteKillerAddonCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var killer = await _killerRepository.GetKiller(request.IdKiller);

                if (killer is null)
                    return Result.Failure(new Error(ErrorCode.Delete, "Такой записи не существует."));

                var itemImageKey = killer.KillerAddons.FirstOrDefault(ia => ia.Id == request.IdAddon)?.ImageKey;

                var wasRemoved = killer.RemoveAddon(request.IdAddon);

                if (!wasRemoved)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Улучшение для удаления не найдено у данного киллера."));

                if (itemImageKey is not null)
                    await _fileStorageService.DeleteImageAsync($"{FileStoragePaths.KillerAddons(killer.Name)}/{itemImageKey.Value}", cancellationToken);

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