using System;
using System.Collections.Generic;

namespace DBDAnalytics.Infrastructure.Models;

public partial class WhoPlacedMap
{
    public int IdWhoPlacedMap { get; set; }

    public string WhoPlacedMapName { get; set; } = null!;

    public virtual ICollection<GameStatistic> GameStatisticIdWhoPlacedMapNavigations { get; set; } = new List<GameStatistic>();

    public virtual ICollection<GameStatistic> GameStatisticIdWhoPlacedMapWinNavigations { get; set; } = new List<GameStatistic>();
}
