using DBDAnalytics.CatalogService.Domain.Models;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Application.Common.Repository
{
    public interface ITypeDeathRepository : IBaseRepository<TypeDeath>
    {
        Task<TypeDeath> Get(int id);
        Task<bool> Exist(string name);
    }
}