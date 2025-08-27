using DBDAnalytics.CatalogService.Domain.Models;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Application.Common.Repository
{
    public interface IPlayerAssociationRepository : IBaseRepository<PlayerAssociation>
    {
        Task<PlayerAssociation> Get(int id);
        Task<bool> Exist(string name);
    }
}