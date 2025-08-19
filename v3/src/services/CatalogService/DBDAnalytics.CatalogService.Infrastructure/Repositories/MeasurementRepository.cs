using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.CatalogService.Infrastructure.Repositories
{
    public sealed class MeasurementRepository(IApplicationDbContext context)
        : BaseRepository<Measurement, IApplicationDbContext>(context), IMeasurementRepository
    {
        public async Task<Measurement> GetMeasurement(Guid id) => await _context.Measurements.Include(m => m.Maps).FirstOrDefaultAsync(m => m.Id == id);
        
        public async Task<bool> Exist(string name) => await _context.Measurements.AnyAsync(x => x.Name == name);
        
        public async Task<bool> ExistMap(Guid measurementId, string mapName) => await _context.Maps.AnyAsync(m => m.MeasurementId == measurementId && m.Name == mapName);
    }
}