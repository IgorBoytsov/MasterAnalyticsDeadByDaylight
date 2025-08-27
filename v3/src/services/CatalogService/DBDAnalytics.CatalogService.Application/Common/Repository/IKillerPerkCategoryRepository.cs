using DBDAnalytics.CatalogService.Domain.Models;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Application.Common.Repository
{
    public interface IKillerPerkCategoryRepository : IBaseRepository<KillerPerkCategory>
    {
        Task<KillerPerkCategory> Get(int id);
        Task<bool> Exist(string name);
    }
}