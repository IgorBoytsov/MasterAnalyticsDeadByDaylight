using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.Shared.Domain.Constants;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.Delete
{
    public sealed class DeleteOfferingCommandHandler(
        IApplicationDbContext context,
        IOfferingRepository offeringRepository,
        IFileStorageService fileStorageService) : IRequestHandler<DeleteOfferingCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IOfferingRepository _offeringRepository = offeringRepository;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        public async Task<Result> Handle(DeleteOfferingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var offering = await _offeringRepository.Get(request.Id);

                if (offering is null)
                    return Result.Failure(new Error(ErrorCode.Delete, "Такой записи не существует."));

                var roleId = offering.RoleId;
                var offeringImageKey = offering.ImageKey;
                var imageKeyPath = string.Empty;

                _offeringRepository.Remove(offering);

                if ((int)Shared.Domain.Enums.Roles.Survivor == roleId) imageKeyPath = $"{FileStoragePaths.OfferingSurvivor}/{offeringImageKey}";
                if ((int)Shared.Domain.Enums.Roles.Killer == roleId) imageKeyPath = $"{FileStoragePaths.OfferingKiller}/{offeringImageKey}";
                if ((int)Shared.Domain.Enums.Roles.General == roleId) imageKeyPath = $"{FileStoragePaths.OfferingGeneral}/{offeringImageKey}";

                if (!string.IsNullOrWhiteSpace(imageKeyPath)) 
                    await _fileStorageService.DeleteImageAsync(imageKeyPath, cancellationToken);

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