using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Infrastructure.Repositories
{
    internal sealed class PlayerAssociationRepository(IApplicationDbContext context)
        : BaseRepository<PlayerAssociation, IApplicationDbContext>(context), IPlayerAssociationRepository
    {
        public async Task<PlayerAssociation> Get(int id) => await _context.PlayerAssociations.FirstOrDefaultAsync(pa => pa.Id == id);
        public async Task<bool> Exist(string name) => await _context.PlayerAssociations.AnyAsync(p => p.Name == name);
    }
}