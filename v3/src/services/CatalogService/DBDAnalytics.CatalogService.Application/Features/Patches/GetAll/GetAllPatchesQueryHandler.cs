using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Patches.GetAll
{
    public sealed class GetAllPatchesQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllPatchesQuery, List<PatchResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<PatchResponse>> Handle(GetAllPatchesQuery request, CancellationToken cancellationToken)
            => await _context.Patches
                .AsNoTracking()
                    .ProjectTo<PatchResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}