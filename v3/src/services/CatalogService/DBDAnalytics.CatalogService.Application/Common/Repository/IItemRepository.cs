using DBDAnalytics.CatalogService.Domain.Models;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Application.Common.Repository
{
    public interface IItemRepository : IBaseRepository<Item>
    {
        Task<bool> Exist(string name);
        Task<Item> GetItem(Guid id);
    }
}