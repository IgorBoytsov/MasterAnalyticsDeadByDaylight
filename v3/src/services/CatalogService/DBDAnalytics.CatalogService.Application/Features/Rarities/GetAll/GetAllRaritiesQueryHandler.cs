using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Rarities.GetAll
{
    public sealed class GetAllRaritiesQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllRaritiesQuery, List<RarityResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<RarityResponse>> Handle(GetAllRaritiesQuery request, CancellationToken cancellationToken)
            => await _context.Rarities
                .AsNoTracking()
                    .ProjectTo<RarityResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}