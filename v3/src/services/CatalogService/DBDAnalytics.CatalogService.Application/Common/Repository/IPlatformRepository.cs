using DBDAnalytics.CatalogService.Domain.Models;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Application.Common.Repository
{
    public interface IPlatformRepository : IBaseRepository<Platform>
    {
        Task<bool> Exist(string name);
    }
}