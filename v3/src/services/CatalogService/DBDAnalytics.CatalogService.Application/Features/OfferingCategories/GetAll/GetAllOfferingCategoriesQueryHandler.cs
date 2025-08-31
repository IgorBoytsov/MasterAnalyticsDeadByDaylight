using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.Offering;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.OfferingCategories.GetAll
{
    public sealed class GetAllOfferingCategoriesQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllOfferingCategoriesQuery, List<OfferingCategoryResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<OfferingCategoryResponse>> Handle(GetAllOfferingCategoriesQuery request, CancellationToken cancellationToken)
            => await _context.OfferingCategories.
                AsNoTracking()
                    .ProjectTo<OfferingCategoryResponse>(_mapper.ConfigurationProvider).
                        ToListAsync(cancellationToken);
    }
}