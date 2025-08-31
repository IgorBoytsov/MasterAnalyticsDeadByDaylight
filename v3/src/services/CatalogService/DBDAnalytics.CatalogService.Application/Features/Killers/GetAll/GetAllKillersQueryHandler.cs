using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.Killers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.GetAll
{
    public sealed class GetAllKillersQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllKillersQuery, List<KillerSoloResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<KillerSoloResponse>> Handle(GetAllKillersQuery request, CancellationToken cancellationToken)
            => await _context.Killers
                .AsNoTracking()
                    .ProjectTo<KillerSoloResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}