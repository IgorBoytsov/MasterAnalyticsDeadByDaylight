using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.Offering;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.GetAllForSurvivor
{
    public sealed class GetAllOfferingForSurvivorQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllOfferingForSurvivorQuery, List<OfferingResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<OfferingResponse>> Handle(GetAllOfferingForSurvivorQuery request, CancellationToken cancellationToken)
            => await _context.Offerings
                .AsNoTracking()
                    .Where(o => o.RoleId == (int)Shared.Contracts.Enums.Roles.Survivor || o.RoleId == (int)Shared.Contracts.Enums.Roles.General)
                        .ProjectTo<OfferingResponse>(_mapper.ConfigurationProvider)
                            .ToListAsync(cancellationToken);
    }
}