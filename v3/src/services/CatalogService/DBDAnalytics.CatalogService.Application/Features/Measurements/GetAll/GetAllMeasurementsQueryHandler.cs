using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.Maps;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.GetAll
{
    public sealed class GetAllMeasurementsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllMeasurementsQuery, List<MeasurementSoloResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<MeasurementSoloResponse>> Handle(GetAllMeasurementsQuery request, CancellationToken cancellationToken)
            => await _context.Measurements
                .AsNoTracking()
                    .ProjectTo<MeasurementSoloResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}