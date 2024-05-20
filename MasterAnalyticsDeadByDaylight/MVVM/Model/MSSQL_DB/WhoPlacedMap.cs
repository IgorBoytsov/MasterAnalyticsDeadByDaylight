using System;
using System.Collections.Generic;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

public partial class WhoPlacedMap
{
    public int IdWhoPlacedMap { get; set; }

    public string WhoPlacedMapName { get; set; }

    public virtual ICollection<GameStatistic> GameStatisticIdWhoPlacedMapNavigations { get; set; } = new List<GameStatistic>();

    public virtual ICollection<GameStatistic> GameStatisticIdWhoPlacedMapWinNavigations { get; set; } = new List<GameStatistic>();
}
