using System.Linq.Expressions;

namespace MasterAnalyticsDeadByDaylight.Services.DatabaseServices
{
    public interface IDataService
    {
        Task<IEnumerable<T>> GetAllDataAsync<T>(Func<IQueryable<T>, IQueryable<T>> include = null) where T : class;
        Task<List<T>> GetAllDataInListAsync<T>(Func<IQueryable<T>, IQueryable<T>> include = null) where T : class;
        Task<T> GetByIdAsync<T>(int id, string nameProperty, Func<IQueryable<T>, IQueryable<T>> include = null) where T : class;
        Task<T> FindByValueAsync<T>(string propertyName, object value, Func<IQueryable<T>, IQueryable<T>> include = null) where T : class;
        Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
        Task AddAsync<T>(T entity) where T : class;
        Task UpdateAsync<T>(T entity) where T : class;
        Task RemoveAsync<T>(T entity) where T : class;
        Task RemoveRangeAsync<T>(IEnumerable<T> entities) where T : class;
        Task<T> FindAsync<T>(int id) where T : class;
    }
}
