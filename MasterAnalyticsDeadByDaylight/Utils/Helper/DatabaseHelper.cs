using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterAnalyticsDeadByDaylight.Utils.Helper
{
    public class DatabaseHelper
    {
        //private readonly DbContext _context;

        //public DatabaseHelper(DbContext context)
        //{
        //    _context = context;
        //}

        //public DatabaseHelper() { }

        //public List<T> GetData()
        //{
        //    using (MasterAnalyticsDeadByDaylightDbContext context = new())
        //    {
        //        return _context.Set<T>().ToList();
        //    }
        //}

        //public async Task<List<T>> GetDataAsync()
        //{
        //    return await _context.Set<T>().ToListAsync();
        //}

        //public async Task<List<GameMode>> GetGameModeDataAsync()
        //{
        //    using (MasterAnalyticsDeadByDaylightDbContext context = new())
        //    {
        //        return await context.GameModes.ToListAsync();
        //    }
        //}

        //public async Task<List<GameEvent>> GetEventDataAsync()
        //{
        //    using (MasterAnalyticsDeadByDaylightDbContext context = new())
        //    {
        //        return await context.GameEvents.ToListAsync();
        //    }
        //}
    }
}
