using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.PlayerAssociations.GetAll
{
    public sealed class GetAllPlayerAssociationsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllPlayerAssociationsQuery, List<PlayerAssociationResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<PlayerAssociationResponse>> Handle(GetAllPlayerAssociationsQuery request, CancellationToken cancellationToken)
            => await _context.PlayerAssociations
                .AsNoTracking()
                    .ProjectTo<PlayerAssociationResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}