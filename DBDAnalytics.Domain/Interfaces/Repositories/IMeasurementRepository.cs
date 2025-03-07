using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IMeasurementRepository
    {
        Task<int> CreateAsync(string measurementName, string measurementDescription);
        Task<int> DeleteAsync(int idMeasurement);
        bool Exist(int idMeasurement);
        bool Exist(string measurementName);
        Task<bool> ExistAsync(int idMeasurement);
        Task<bool> ExistAsync(string measurementName);
        IEnumerable<MeasurementDomain> GetAll();
        Task<IEnumerable<MeasurementDomain>> GetAllAsync();
        Task<MeasurementDomain?> GetAsync(int idMeasurement);
        Task<IEnumerable<MeasurementWithMapsDomain>> GetMeasurementsWithMapsAsync();
        Task<MeasurementWithMapsDomain?> GetMeasurementWithMapsAsync(int idMeasurement);
        IEnumerable<MeasurementWithMapsDomain> GetMeasurementsWithMaps();
        MeasurementWithMapsDomain? GetMeasurementWithMaps(int idMeasurement);
        Task<int> UpdateAsync(int idMeasurement, string measurementName, string measurementDescription);
    }
}