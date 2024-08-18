using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterAnalyticsDeadByDaylight.Utils.Helper
{
    public class DataBaseHelper
    {
        public static async Task DeleteEntityAsync<T>(T entity) where T : class
        {
            using (MasterAnalyticsDeadByDaylightDbContext context = new())
            {
                ArgumentNullException.ThrowIfNull(entity);

                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();
            }
            
        }

    }
}
