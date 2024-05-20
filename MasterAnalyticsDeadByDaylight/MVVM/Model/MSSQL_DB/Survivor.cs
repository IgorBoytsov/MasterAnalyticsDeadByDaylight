using System;
using System.Collections.Generic;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

public partial class Survivor
{
    public int IdSurvivor { get; set; }

    public string SurvivorName { get; set; }

    public byte[] SurvivorImage { get; set; }

    public string SurvivorDescription { get; set; }

    public virtual ICollection<SurvivorInfo> SurvivorInfos { get; set; } = new List<SurvivorInfo>();
}
