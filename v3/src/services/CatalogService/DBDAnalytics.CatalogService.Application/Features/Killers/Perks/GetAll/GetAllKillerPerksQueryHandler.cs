using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.Killers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Perks.GetAll
{
    public sealed class GetAllKillerPerksQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllKillerPerksQuery, List<KillerPerkResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<KillerPerkResponse>> Handle(GetAllKillerPerksQuery request, CancellationToken cancellationToken)
            => await _context.KillerPerks
                .AsNoTracking()
                    .ProjectTo<KillerPerkResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}