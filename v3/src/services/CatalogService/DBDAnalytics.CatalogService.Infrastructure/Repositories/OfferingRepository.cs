using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Infrastructure.Repositories
{
    internal sealed class OfferingRepository(IApplicationDbContext context)
        : BaseRepository<Offering, IApplicationDbContext>(context), IOfferingRepository
    {
        public async Task<bool> Exist(string name) => await _context.Offerings.AnyAsync(o => o.Name == name);
    }
}