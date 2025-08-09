using System;
using System.Collections.Generic;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

public partial class Patch
{
    public int IdPatch { get; set; }

    public string PatchNumber { get; set; }

    public DateOnly PatchDateRelease { get; set; }

    public virtual ICollection<GameStatistic> GameStatistics { get; set; } = new List<GameStatistic>();
}
