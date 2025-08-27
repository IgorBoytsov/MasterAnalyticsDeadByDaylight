using DBDAnalytics.CatalogService.Domain.Models;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Application.Common.Repository
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<Role> Get(int id);
        Task<bool> Exist(string name);
    }
}