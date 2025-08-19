using DBDAnalytics.CatalogService.Domain.Models;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Application.Common.Repository
{
    public interface IMeasurementRepository : IBaseRepository<Measurement>
    {
        Task<Measurement> GetMeasurement(Guid id);
        Task<bool> ExistMap(Guid measurementId, string mapName);
        Task<bool> Exist(string name);
    }
}