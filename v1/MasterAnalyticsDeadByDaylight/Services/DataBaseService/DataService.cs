using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MasterAnalyticsDeadByDaylight.Services.DatabaseServices
{
    public class DataService(Func<MasterAnalyticsDeadByDaylightDbContext> contextFactory) : IDataService
    {
        private readonly Func<MasterAnalyticsDeadByDaylightDbContext> _contextFactory = contextFactory;

        #region Асинхронные методы

        public async Task<IEnumerable<T>> GetAllDataAsync<T>(Func<IQueryable<T>, IQueryable<T>> include = null) where T : class
        {
            using (var context = _contextFactory())
            {
                IQueryable<T> query = context.Set<T>();

                if (include != null)
                {
                    query = include(query);
                }

                return await query.ToListAsync();
            }
        }

        public async Task<List<T>> GetAllDataInListAsync<T>(Func<IQueryable<T>, IQueryable<T>> include = null) where T : class
        {
            using (var context = _contextFactory())
            {
                IQueryable<T> query = context.Set<T>();

                if (include != null)
                {
                    query = include(query);
                }

                return await query.ToListAsync();
            }
        }

        public async Task<T> GetByIdAsync<T>(int id, string nameProperty, Func<IQueryable<T>, IQueryable<T>> include = null) where T : class
        {
            using (var context = _contextFactory())
            {
                IQueryable<T> query = context.Set<T>();

                if (include != null)
                {
                    query = include(query);
                }

                try
                {
                    return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, nameProperty) == id);
                }
                catch (Exception)
                {
                    throw new Exception($"Свойства {nameProperty} не существует");
                }
            }
        }

        public async Task<T> FindByValueAsync<T>(string propertyName, object value, Func<IQueryable<T>, IQueryable<T>> include = null) where T : class
        {
            using (var context = _contextFactory())
            {
                IQueryable<T> query = context.Set<T>();

                if (include != null)
                {
                    query = include(query);
                }

                try
                {
                    return await query.FirstOrDefaultAsync(e => EF.Property<object>(e, propertyName).Equals(value));
                }
                catch (Exception)
                {
                    throw new Exception($"Свойства {propertyName} не существует");
                }
            }
        }

        public async Task<T> FindAsync<T>(int id) where T : class
        {
            using (var context = _contextFactory())
            {
                return await context.Set<T>().FindAsync(id);
            }
        }

        public async Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            using (var context = _contextFactory())
            {
                return await context.Set<T>().AnyAsync(predicate);
            }
        }

        public async Task AddAsync<T>(T entity) where T : class
        {
            using (var context = _contextFactory())
            {
                await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync<T>(T entity) where T : class
        {
            using (var context = _contextFactory())
            {
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveRangeAsync<T>(IEnumerable<T> entities) where T : class
        {
            using (var context = _contextFactory())
            {
                context.Set<T>().RemoveRange(entities);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync<T>(T entity) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Объект не найден");
            }

            using (var context = _contextFactory())
            {
                context.Set<T>().Update(entity);
                await context.SaveChangesAsync();
            }
        }

        #endregion

        #region Обычные методы

        public IEnumerable<T> GetAllData<T>(Func<IQueryable<T>, IQueryable<T>> include = null) where T : class
        {
            using (var context = _contextFactory())
            {
                IQueryable<T> query = context.Set<T>();

                if (include != null)
                {
                    query = include(query);
                }

                return query.ToList();
            }
        }

        public List<T> GetAllDataInList<T>(Func<IQueryable<T>, IQueryable<T>> include = null) where T : class
        {
            using (var context = _contextFactory())
            {
                IQueryable<T> query = context.Set<T>();

                if (include != null)
                {
                    query = include(query);
                }

                return query.ToList();
            }
        }

        public T GetById<T>(int id, string nameProperty, Func<IQueryable<T>, IQueryable<T>> include = null) where T : class
        {
            using (var context = _contextFactory())
            {
                IQueryable<T> query = context.Set<T>();

                if (include != null)
                {
                    query = include(query);
                }

                try
                {
                    return query.FirstOrDefault(e => EF.Property<int>(e, nameProperty) == id);
                }
                catch (Exception)
                {
                    throw new Exception($"Свойства {nameProperty} не существует");
                }
            }
        }

        public T FindByValue<T>(string propertyName, object value, Func<IQueryable<T>, IQueryable<T>> include = null) where T : class
        {
            using (var context = _contextFactory())
            {
                IQueryable<T> query = context.Set<T>();

                if (include != null)
                {
                    query = include(query);
                }

                try
                {
                    return query.FirstOrDefault(e => EF.Property<object>(e, propertyName).Equals(value));
                }
                catch (Exception)
                {
                    throw new Exception($"Свойства {propertyName} не существует");
                }
            }
        }

        public T Find<T>(int id) where T : class
        {
            using (var context = _contextFactory())
            {
                return context.Set<T>().Find(id);
            }
        }

        public int Count<T>(Func<IQueryable<T>, IQueryable<T>> include = null) where T : class
        {
            using (var context = _contextFactory())
            {
                IQueryable<T> query = context.Set<T>();

                if (include != null)
                {
                    query = include(query);
                }

                return query.Count();
            }
        }

        public bool Exists<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            using (var context = _contextFactory())
            {
                return context.Set<T>().Any(predicate);
            }
        }

        public void Add<T>(T entity) where T : class
        {
            using (var context = _contextFactory())
            {
                context.Set<T>().Add(entity);
                context.SaveChanges();
            }
        }

        public void Remove<T>(T entity) where T : class
        {
            using (var context = _contextFactory())
            {
                context.Set<T>().Remove(entity);
                context.SaveChanges();
            }
        }

        public void RemoveRange<T>(IEnumerable<T> entities) where T : class
        {
            using (var context = _contextFactory())
            {
                context.Set<T>().RemoveRange(entities);
                context.SaveChanges();
            }
        }

        public void Update<T>(T entity) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Объект не найден");
            }

            using (var context = _contextFactory())
            {
                context.Set<T>().Update(entity);
                context.SaveChanges();
            }
        }

        #endregion
    }
}