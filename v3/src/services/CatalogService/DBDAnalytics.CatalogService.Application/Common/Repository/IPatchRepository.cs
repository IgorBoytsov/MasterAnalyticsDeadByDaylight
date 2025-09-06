using DBDAnalytics.CatalogService.Domain.Models;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Application.Common.Repository
{
    public interface IPatchRepository : IBaseRepository<Patch>
    {
        Task<Patch> Get(int id);
        Task<bool> Exist(string name);
        Task<bool> ExistDate(DateTime date);
    }
}