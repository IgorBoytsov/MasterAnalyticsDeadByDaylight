using DBDAnalytics.CatalogService.Domain.Models;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Application.Common.Repository
{
    public interface IOfferingRepository : IBaseRepository<Offering>
    {
        Task<bool> Exist(string name);
    }
}