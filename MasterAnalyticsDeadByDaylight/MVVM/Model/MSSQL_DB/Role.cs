using System;
using System.Collections.Generic;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

public partial class Role
{
    public int IdRole { get; set; }

    public string RoleName { get; set; }

    public virtual ICollection<Offering> Offerings { get; set; } = new List<Offering>();
}
