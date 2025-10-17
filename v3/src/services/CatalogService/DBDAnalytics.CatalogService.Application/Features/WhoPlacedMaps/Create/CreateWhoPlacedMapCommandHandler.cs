using AutoMapper;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.Shared.Contracts.Responses.Maps;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.WhoPlacedMaps.Create
{
    public sealed class CreateWhoPlacedMapCommandHandler(
        IApplicationDbContext context,
        IWhoPlacedMapRepository whoPlacedMapRepository,
        IMapper mapper) : IRequestHandler<CreateWhoPlacedMapCommand, Result<WhoPlacedMapResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        public readonly IWhoPlacedMapRepository _whoPlacedMapRepository = whoPlacedMapRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<WhoPlacedMapResponse>> Handle(CreateWhoPlacedMapCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var whoPlacedMap = WhoPlacedMap.Create(request.OldId, request.Name);

                await _whoPlacedMapRepository.AddAsync(whoPlacedMap, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                var dto = _mapper.Map<WhoPlacedMapResponse>(whoPlacedMap);

                return Result<WhoPlacedMapResponse>.Success(dto);
            }
            catch (DomainException ex)
            {
                return Result<WhoPlacedMapResponse>.Failure(ex.Error);
            }
            catch (Exception ex)
            {
                return Result<WhoPlacedMapResponse>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи об том, кто поставил карту {ex.Message} {ex.InnerException?.Message}"));
            }
        }
    }
}