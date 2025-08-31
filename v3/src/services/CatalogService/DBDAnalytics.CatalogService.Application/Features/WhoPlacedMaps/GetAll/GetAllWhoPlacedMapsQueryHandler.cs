using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.Maps;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.WhoPlacedMaps.GetAll
{
    public sealed class GetAllWhoPlacedMapsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllWhoPlacedMapsQuery, List<WhoPlacedMapResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<WhoPlacedMapResponse>> Handle(GetAllWhoPlacedMapsQuery request, CancellationToken cancellationToken)
            => await _context.WhoPlacedMaps
                .AsNoTracking()
                    .ProjectTo<WhoPlacedMapResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}