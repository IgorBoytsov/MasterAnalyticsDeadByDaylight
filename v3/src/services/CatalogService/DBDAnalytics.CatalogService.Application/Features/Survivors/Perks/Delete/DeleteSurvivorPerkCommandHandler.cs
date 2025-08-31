using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.Shared.Domain.Constants;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.Perks.Delete
{
    public sealed class DeleteSurvivorPerkCommandHandler(
        IApplicationDbContext context,
        ISurvivorRepository survivorRepository,
        IFileStorageService fileStorageService) : IRequestHandler<DeleteSurvivorPerkCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly ISurvivorRepository _survivorRepository = survivorRepository;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<Result> Handle(DeleteSurvivorPerkCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var survivor = await _survivorRepository.GetSurvivor(request.SurvivorId);

                if (survivor is null)
                    return Result.Failure(new Error(ErrorCode.Delete, "Такой записи не существует."));

                var perkImageKey = survivor.SurvivorPerks.FirstOrDefault(ia => ia.Id == request.PerkId)?.ImageKey;

                var wasRemoved = survivor.RemovePerk(request.PerkId);

                if (!wasRemoved)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Перк для удаления не найден у данного выжившего."));

                if (perkImageKey is not null)
                    await _fileStorageService.DeleteImageAsync($"{FileStoragePaths.SurvivorPerks(survivor.Name)}/{perkImageKey.Value}", cancellationToken);

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