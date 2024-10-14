using System;
using System.Collections.Generic;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

public partial class KillerPerkCategory
{
    public int IdKillerPerkCategory { get; set; }

    public string CategoryName { get; set; }

    public virtual ICollection<KillerPerk> KillerPerks { get; set; } = new List<KillerPerk>();
}
