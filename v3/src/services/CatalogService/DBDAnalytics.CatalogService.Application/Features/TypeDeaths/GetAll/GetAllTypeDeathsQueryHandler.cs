using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.TypeDeaths.GetAll
{
    public sealed class GetAllTypeDeathsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllTypeDeathsQuery, List<TypeDeathResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<TypeDeathResponse>> Handle(GetAllTypeDeathsQuery request, CancellationToken cancellationToken)
            => await _context.TypeDeaths
                .AsNoTracking()
                    .ProjectTo<TypeDeathResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}