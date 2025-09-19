using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.Shared.Contracts.Constants;
using MediatR;
using Shared.Kernel.Results;

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

                var imageKeyPath = FileStoragePaths.GetOfferingPathForRole(offering.RoleId);

                _offeringRepository.Remove(offering);

                if (!string.IsNullOrWhiteSpace(imageKeyPath)) 
                    await _fileStorageService.DeleteImageAsync($"{imageKeyPath}/{offering.ImageKey}", cancellationToken);

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