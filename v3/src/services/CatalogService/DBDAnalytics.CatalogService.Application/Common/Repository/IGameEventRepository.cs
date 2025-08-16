using DBDAnalytics.CatalogService.Domain.Models;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Application.Common.Repository
{
    public interface IGameEventRepository : IBaseRepository<GameEvent>
    {
        Task<bool> Exist(string name);
    }
}