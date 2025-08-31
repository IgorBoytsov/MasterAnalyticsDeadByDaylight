using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.GetAll
{
    public sealed class GetAllSurvivorsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllSurvivorsQuery, List<SurvivorSoloResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<SurvivorSoloResponse>> Handle(GetAllSurvivorsQuery request, CancellationToken cancellationToken)
            => await _context.Survivors
                .AsNoTracking()
                    .ProjectTo<SurvivorSoloResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}