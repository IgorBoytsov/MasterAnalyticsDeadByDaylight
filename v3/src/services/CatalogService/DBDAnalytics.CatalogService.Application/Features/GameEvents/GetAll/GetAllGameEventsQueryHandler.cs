using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.Match;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.GameEvents.GetAll
{
    public sealed class GetAllGameEventsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllGameEventsQuery, List<GameEventResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<GameEventResponse>> Handle(GetAllGameEventsQuery request, CancellationToken cancellationToken)
            => await _context.GameEvents
                .AsNoTracking()
                    .ProjectTo<GameEventResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}