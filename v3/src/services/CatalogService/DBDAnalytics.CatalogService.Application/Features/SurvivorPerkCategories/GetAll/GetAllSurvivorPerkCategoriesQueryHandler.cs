using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.SurvivorPerkCategories.GetAll
{
    public sealed class GetAllSurvivorPerkCategoriesQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllSurvivorPerkCategoriesQuery, List<SurvivorPerkCategoryResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<SurvivorPerkCategoryResponse>> Handle(GetAllSurvivorPerkCategoriesQuery request, CancellationToken cancellationToken)
            => await _context.SurvivorPerkCategories
                .AsNoTracking()
                    .ProjectTo<SurvivorPerkCategoryResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}