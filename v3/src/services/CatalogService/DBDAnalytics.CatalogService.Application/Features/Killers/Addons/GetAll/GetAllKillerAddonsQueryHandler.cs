using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.Killers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Addons.GetAll
{
    public sealed class GetAllKillerAddonsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllKillerAddonsQuery, List<KillerAddonResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<KillerAddonResponse>> Handle(GetAllKillerAddonsQuery request, CancellationToken cancellationToken)
            => await _context.KillerAddons
                .AsNoTracking()
                    .ProjectTo<KillerAddonResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}