using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Items.GetAll
{
    public sealed class GetAllItemsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllItemsQuery, List<ItemSoloResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<ItemSoloResponse>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
            => await _context.Items.AsNoTracking().ProjectTo<ItemSoloResponse>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
    }
}