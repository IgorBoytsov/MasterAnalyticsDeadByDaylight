using AutoMapper;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Platforms.Create
{
    public sealed class CreatePlatformCommandHandler(
        IApplicationDbContext context,
        IPlatformRepository platformRepository,
        IMapper mapper) : IRequestHandler<CreatePlatformCommand, Result<PlatformResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IPlatformRepository _platformRepository = platformRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<PlatformResponse>> Handle(CreatePlatformCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var platform = Platform.Create(request.OldId, request.Name);

                await _platformRepository.AddAsync(platform, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                var dto = _mapper.Map<PlatformResponse>(platform);

                return Result<PlatformResponse>.Success(dto);
            }
            catch (DomainException ex)
            {
                return Result<PlatformResponse>.Failure(ex.Error);
            }
            catch (Exception ex)
            {
                return Result<PlatformResponse>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи об платформе {ex.Message} {ex.InnerException?.Message}"));
            }
        }
    }
}