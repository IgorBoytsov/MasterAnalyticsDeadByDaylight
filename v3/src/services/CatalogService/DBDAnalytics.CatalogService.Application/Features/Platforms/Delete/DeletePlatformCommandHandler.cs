using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Platforms.Delete
{
    public sealed class DeletePlatformCommandHandler(
        IApplicationDbContext context,
        IPlatformRepository platformRepository) : IRequestHandler<DeletePlatformCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IPlatformRepository _platformRepository = platformRepository;

        public async Task<Result> Handle(DeletePlatformCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var platform = await _platformRepository.Get(request.Id);

                if (platform is null)
                    return Result.Failure(new Error(ErrorCode.Delete, "Такой записи не существует."));

                _platformRepository.Remove(platform);

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