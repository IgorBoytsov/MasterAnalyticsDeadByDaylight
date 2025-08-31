using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.Offering;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.GetAll
{
    public sealed class GetAllOfferingQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllOfferingQuery, List<OfferingResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<OfferingResponse>> Handle(GetAllOfferingQuery request, CancellationToken cancellationToken)
            => await _context.Offerings
                .AsNoTracking()
                    .ProjectTo<OfferingResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}