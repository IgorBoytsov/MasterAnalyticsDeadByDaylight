using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Roles.GetAll
{
    public sealed class GetAllRolesQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllRolesQuery, List<RoleResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<RoleResponse>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
            => await _context.Roles
                .AsNoTracking()
                    .ProjectTo<RoleResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}