using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.Maps;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.Maps.GetById
{
    public sealed class GetMapsByMeasurementIdQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetMapsByMeasurementIdQuery, List<MapResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<MapResponse>> Handle(GetMapsByMeasurementIdQuery request, CancellationToken cancellationToken)
            => await _context.Maps
                .AsNoTracking()
                    .Where(m => m.MeasurementId == request.Id)
                        .ProjectTo<MapResponse>(_mapper.ConfigurationProvider)
                            .ToListAsync(cancellationToken);
    }
}