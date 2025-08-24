using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Infrastructure.Repositories
{
    internal sealed class ItemRepository(IApplicationDbContext context)
        : BaseRepository<Item, IApplicationDbContext>(context), IItemRepository
    {
        public async Task<Item> GetItem(Guid id) => await _context.Items.Include(i => i.ItemAddons).FirstOrDefaultAsync(i => i.Id == id);

        public async Task<bool> Exist(string name) => await _context.Items.AnyAsync(i => i.Name == name);
    }
}