using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.Offering;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.GetAllForKiller
{
    public sealed class GetAllOfferingForKillerQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllOfferingForKillerQuery, List<OfferingResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<OfferingResponse>> Handle(GetAllOfferingForKillerQuery request, CancellationToken cancellationToken)
            => await _context.Offerings
                .AsNoTracking()
                    .Where(o => o.RoleId == (int)Shared.Contracts.Enums.Roles.Killer || o.RoleId == (int)Shared.Contracts.Enums.Roles.General)
                        .ProjectTo<OfferingResponse>(_mapper.ConfigurationProvider)
                            .ToListAsync(cancellationToken);
    }
}
