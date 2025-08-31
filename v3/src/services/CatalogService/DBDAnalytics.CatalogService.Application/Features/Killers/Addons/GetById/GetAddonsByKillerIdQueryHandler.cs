using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.Killers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Addons.GetById
{
    public sealed class GetAddonsByKillerIdQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAddonsByKillerIdQuery, List<KillerAddonResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<KillerAddonResponse>> Handle(GetAddonsByKillerIdQuery request, CancellationToken cancellationToken)
            => await _context.KillerAddons
                .AsNoTracking()
                    .Where(ka => ka.KillerId == request.Id)
                        .ProjectTo<KillerAddonResponse>(_mapper.ConfigurationProvider)
                            .ToListAsync(cancellationToken);
    }
}