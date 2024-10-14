using System;
using System.Collections.Generic;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

public partial class SurvivorPerkCategory
{
    public int IdSurvivorPerkCategory { get; set; }

    public string CategoryName { get; set; }

    public virtual ICollection<SurvivorPerk> SurvivorPerks { get; set; } = new List<SurvivorPerk>();
}
