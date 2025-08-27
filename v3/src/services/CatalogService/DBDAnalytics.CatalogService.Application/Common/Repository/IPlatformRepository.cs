using DBDAnalytics.CatalogService.Domain.Models;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Application.Common.Repository
{
    public interface IPlatformRepository : IBaseRepository<Platform>
    {
        Task<Platform> Get(int id);
        Task<bool> Exist(string name);
    }
}