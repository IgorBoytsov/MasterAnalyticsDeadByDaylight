using DBDAnalytics.CatalogService.Domain.Models;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Application.Common.Repository
{
    public interface IKillerRepository : IBaseRepository<Killer>
    {
        Task<Killer> GetKiller(Guid id);

        Task<bool> ExistName(string name);
        Task<bool> ExistAddon(Guid idKiller, string addonName);
        Task<bool> ExistPerk(Guid idKiller, string perkName);
    }
}