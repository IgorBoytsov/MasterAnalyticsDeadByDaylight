using System;
using System.Collections.Generic;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

public partial class GameEvent
{
    public int IdGameEvent { get; set; }

    public string GameEventName { get; set; }

    public string GameEventDescription { get; set; }

    public virtual ICollection<GameStatistic> GameStatistics { get; set; } = new List<GameStatistic>();
}
