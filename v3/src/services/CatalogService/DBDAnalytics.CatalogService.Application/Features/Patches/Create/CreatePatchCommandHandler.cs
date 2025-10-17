using AutoMapper;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.Shared.Contracts.Responses;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Patches.Create
{
    public sealed class CreatePatchCommandHandler(
        IApplicationDbContext context,
        IPatchRepository patchRepository,
        IMapper mapper) : IRequestHandler<CreatePatchCommand, Result<PatchResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IPatchRepository _patchRepository = patchRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<PatchResponse>> Handle(CreatePatchCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var patch = Patch.Create(request.OldId, request.Name, request.Date);

                await _patchRepository.AddAsync(patch, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                var dto = _mapper.Map<PatchResponse>(patch);

                return Result<PatchResponse>.Success(dto);
            }
            catch (DomainException ex)
            {
                return Result<PatchResponse>.Failure(ex.Error);
            }
            catch (Exception ex)
            {
                return Result<PatchResponse>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи об патче {ex.Message} {ex.InnerException?.Message}"));
            }
        }
    }
}