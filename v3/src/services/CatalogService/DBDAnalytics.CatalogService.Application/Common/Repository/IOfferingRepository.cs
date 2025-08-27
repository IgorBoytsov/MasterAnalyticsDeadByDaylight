using DBDAnalytics.CatalogService.Domain.Models;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Application.Common.Repository
{
    public interface IOfferingRepository : IBaseRepository<Offering>
    {
        Task<Offering> Get(Guid id);
        Task<bool> Exist(string name);
    }
}