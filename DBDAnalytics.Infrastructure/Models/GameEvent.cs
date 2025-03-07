using System;
using System.Collections.Generic;

namespace DBDAnalytics.Infrastructure.Models;

public partial class GameEvent
{
    public int IdGameEvent { get; set; }

    public string GameEventName { get; set; } = null!;

    public string? GameEventDescription { get; set; }

    public virtual ICollection<GameStatistic> GameStatistics { get; set; } = new List<GameStatistic>();
}
