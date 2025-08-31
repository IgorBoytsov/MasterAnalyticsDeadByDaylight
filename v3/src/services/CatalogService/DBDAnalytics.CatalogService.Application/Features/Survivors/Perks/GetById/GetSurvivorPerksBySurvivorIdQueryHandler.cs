using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.Perks.GetById
{
    public sealed class GetSurvivorPerksBySurvivorIdQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetSurvivorPerksBySurvivorIdQuery, List<SurvivorPerkResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<SurvivorPerkResponse>> Handle(GetSurvivorPerksBySurvivorIdQuery request, CancellationToken cancellationToken)
            => await _context.SurvivorPerks
                .AsNoTracking()
                    .Where(sp => sp.SurvivorId == request.Id)
                        .ProjectTo<SurvivorPerkResponse>(_mapper.ConfigurationProvider)
                            .ToListAsync(cancellationToken);
    }
}