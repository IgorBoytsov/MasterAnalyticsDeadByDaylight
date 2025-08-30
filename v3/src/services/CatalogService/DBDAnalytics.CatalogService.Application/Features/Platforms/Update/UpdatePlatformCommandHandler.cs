using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Platform;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Platforms.Update
{
    public sealed class UpdatePlatformCommandHandler(
        IApplicationDbContext context,
        IPlatformRepository platformRepository) : IRequestHandler<UpdatePlatformCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IPlatformRepository _platformRepository = platformRepository;

        public async Task<Result> Handle(UpdatePlatformCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var platform = await _platformRepository.Get(request.PlatformId);

                if (platform is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данной записи не найдено."));

                platform.UpdateName(PlatformName.Create(request.Name));

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