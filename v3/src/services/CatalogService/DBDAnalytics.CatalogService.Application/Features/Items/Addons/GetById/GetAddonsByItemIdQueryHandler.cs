using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Items.Addons.GetById
{
    public sealed class GetAddonsByItemIdQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAddonsByItemIdQuery, List<ItemAddonResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<ItemAddonResponse>> Handle(GetAddonsByItemIdQuery request, CancellationToken cancellationToken)
            => await _context.ItemsAddon.AsNoTracking()
                .Where(ia => ia.ItemId == request.Id)
                    .ProjectTo<ItemAddonResponse>(_mapper.ConfigurationProvider).
                        ToListAsync(cancellationToken);
    }
}