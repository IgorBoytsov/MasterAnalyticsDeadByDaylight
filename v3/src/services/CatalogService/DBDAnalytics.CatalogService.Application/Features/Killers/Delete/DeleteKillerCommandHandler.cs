using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.Shared.Contracts.Constants;
using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Delete
{
    public sealed class DeleteKillerCommandHandler(
        IApplicationDbContext context,
        IKillerRepository killerRepository,
        IFileStorageService fileStorageService) : IRequestHandler<DeleteKillerCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IKillerRepository _killerRepository = killerRepository;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<Result> Handle(DeleteKillerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var killerAddonImageKeys = new List<ImageKey?>();
                var killerPerkImageKeys = new List<ImageKey?>();

                var killer = await _killerRepository.GetKiller(request.Id);

                if (killer is null)
                    return Result.Failure(new Error(ErrorCode.Delete, "Такой записи не существует."));

                var killerName = killer.Name;
                var killerImageKey = killer.KillerImageKey;
                var killerAbilityImageKey = killer.AbilityImageKey;

                foreach (var killerAddon in killer.KillerAddons)
                    killerAddonImageKeys.Add(killerAddon.ImageKey);

                foreach (var killerPerk in killer.KillerPerks)
                    killerPerkImageKeys.Add(killerPerk.ImageKey);

                _killerRepository.Remove(killer);

                await _fileStorageService.DeleteImageAsync($"{FileStoragePaths.KillerPortraits}/{killerImageKey}", cancellationToken);
                await _fileStorageService.DeleteImageAsync($"{FileStoragePaths.KillerAbilities}/{killerAbilityImageKey}", cancellationToken);

                foreach (var keyAddon in killerAddonImageKeys)
                    await _fileStorageService.DeleteImageAsync($"{FileStoragePaths.KillerAddons(killerName)}/{keyAddon}", cancellationToken);

                foreach (var keyPerk in killerPerkImageKeys)
                    await _fileStorageService.DeleteImageAsync($"{FileStoragePaths.KillerPerks(killerName)}/{keyPerk}", cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(new Error(ErrorCode.Delete, $"Произошла непредвиденная ошибка на стороне сервера."));
            }
        }
    }
}