using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.Shared.Contracts.Responses.Killers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.KillerPerkCategories.GetAll
{
    public sealed class GetAllKillerPerkCategoryQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllKillerPerkCategoriesQuery, List<KillerPerkCategoryResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<KillerPerkCategoryResponse>> Handle(GetAllKillerPerkCategoriesQuery request, CancellationToken cancellationToken)
            => await _context.KillerPerkCategories
                .AsNoTracking()
                    .ProjectTo<KillerPerkCategoryResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}