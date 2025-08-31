using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.Killers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Perks.GetById
{
    public sealed class GetPerksByKillerIdQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetPerksByKillerIdQuery, List<KillerPerkResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<KillerPerkResponse>> Handle(GetPerksByKillerIdQuery request, CancellationToken cancellationToken)
            => await _context.KillerPerks
                .AsNoTracking()
                    .Where(kp => kp.KillerId == request.Id)
                        .ProjectTo<KillerPerkResponse>(_mapper.ConfigurationProvider)
                            .ToListAsync(cancellationToken);
    }
}