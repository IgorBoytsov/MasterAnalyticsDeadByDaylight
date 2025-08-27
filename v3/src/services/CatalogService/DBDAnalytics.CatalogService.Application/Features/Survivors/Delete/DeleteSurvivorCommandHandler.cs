using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.Shared.Domain.Constants;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.Delete
{
    public sealed class DeleteSurvivorCommandHandler(
        IApplicationDbContext context,
        ISurvivorRepository survivorRepository,
        IFileStorageService fileStorageService) : IRequestHandler<DeleteSurvivorCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly ISurvivorRepository _survivorRepository = survivorRepository;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<Result> Handle(DeleteSurvivorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var survivorPerkImageKeys = new List<ImageKey?>();

                var survivor = await _survivorRepository.GetSurvivor(request.Id);

                if (survivor is null)
                    return Result.Failure(new Error(ErrorCode.Delete, "Такой записи не существует."));

                var survivorName = survivor.Name;
                var survivorImageKey = survivor.ImageKey;

                foreach (var killerPerk in survivor.SurvivorPerks)
                    survivorPerkImageKeys.Add(killerPerk.ImageKey);

                _survivorRepository.Remove(survivor);

                await _fileStorageService.DeleteImageAsync($"{FileStoragePaths.SurvivorPortraits}/{survivorImageKey}", cancellationToken);

                foreach (var keyPerk in survivorPerkImageKeys)
                    await _fileStorageService.DeleteImageAsync($"{FileStoragePaths.SurvivorPerks(survivorName)}/{keyPerk}", cancellationToken);

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