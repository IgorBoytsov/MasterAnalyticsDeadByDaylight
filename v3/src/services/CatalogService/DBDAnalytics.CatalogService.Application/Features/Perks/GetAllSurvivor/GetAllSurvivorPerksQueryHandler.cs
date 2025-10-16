using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Perks.GetAllSurvivor
{
    public sealed class GetAllSurvivorPerks(
    IApplicationDbContext context,
    IMapper mapper) : IRequestHandler<GetAllSurvivorPerksQuery, List<SurvivorPerkResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<SurvivorPerkResponse>> Handle(GetAllSurvivorPerksQuery request, CancellationToken cancellationToken)
            => await _context.SurvivorPerks
                .AsNoTracking()
                    .ProjectTo<SurvivorPerkResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}