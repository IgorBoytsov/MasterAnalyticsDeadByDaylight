using DBDAnalytics.CatalogService.Domain.Models;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Application.Common.Repository
{
    public interface IGameModeRepository : IBaseRepository<GameMode>
    {
        Task<GameMode> Get(int id);
        Task<bool> Exist(string name);
    }
}