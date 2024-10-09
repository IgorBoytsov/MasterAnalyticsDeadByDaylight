using System;
using System.Collections.Generic;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

public partial class OfferingCategory
{
    public int IdCategory { get; set; }

    public string CategoryName { get; set; }

    public virtual ICollection<Offering> Offerings { get; set; } = new List<Offering>();
}
