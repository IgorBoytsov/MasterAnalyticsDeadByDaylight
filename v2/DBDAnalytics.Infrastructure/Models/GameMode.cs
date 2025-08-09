using System;
using System.Collections.Generic;

namespace DBDAnalytics.Infrastructure.Models;

public partial class GameMode
{
    public int IdGameMode { get; set; }

    public string GameModeName { get; set; } = null!;

    public string? GameModeDescription { get; set; }

    public virtual ICollection<GameStatistic> GameStatistics { get; set; } = new List<GameStatistic>();
}
