using DBDAnalytics.CatalogService.Domain.Models;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Application.Common.Repository
{
    public interface ISurvivorRepository : IBaseRepository<Survivor>
    {
        Task<bool> Exist(string name);
        Task<Survivor> GetSurvivor(Guid id);
    }
}