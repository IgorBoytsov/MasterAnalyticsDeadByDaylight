using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.Maps;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Maps.GetAll
{
    public sealed class GetAllMapsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllMapsQuery, List<MapResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<MapResponse>> Handle(GetAllMapsQuery request, CancellationToken cancellationToken)
            => await _context.Maps
                .AsNoTracking()
                    .ProjectTo<MapResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}