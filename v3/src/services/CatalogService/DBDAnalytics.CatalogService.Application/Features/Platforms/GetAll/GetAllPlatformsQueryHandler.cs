using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Platforms.GetAll
{
    public sealed class GetAllPlatformsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllPlatformsQuery, List<PlatformResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<PlatformResponse>> Handle(GetAllPlatformsQuery request, CancellationToken cancellationToken)
            => await _context.Platforms
                .AsNoTracking()
                    .ProjectTo<PlatformResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}