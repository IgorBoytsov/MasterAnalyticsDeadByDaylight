using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.Match;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.GameModes.GetAll
{
    public sealed class GetAllGameModesQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllGameModesQuery, List<GameModeResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<GameModeResponse>> Handle(GetAllGameModesQuery request, CancellationToken cancellationToken)
            => await _context.GameModes
                .AsNoTracking()
                    .ProjectTo<GameModeResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}