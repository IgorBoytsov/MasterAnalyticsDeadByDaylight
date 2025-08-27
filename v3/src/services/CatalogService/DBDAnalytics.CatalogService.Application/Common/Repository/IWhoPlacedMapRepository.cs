using DBDAnalytics.CatalogService.Domain.Models;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Application.Common.Repository
{
    public interface IWhoPlacedMapRepository : IBaseRepository<WhoPlacedMap>
    {
        Task<WhoPlacedMap> Get(int id);
        Task<bool> Exist(string name);
    }
}