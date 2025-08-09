using System;
using System.Collections.Generic;

namespace DBDAnalytics.Infrastructure.Models;

public partial class Map
{
    public int IdMap { get; set; }

    public string MapName { get; set; } = null!;

    public byte[]? MapImage { get; set; }

    public string? MapDescription { get; set; }

    public int? IdMeasurement { get; set; }

    public virtual ICollection<GameStatistic> GameStatistics { get; set; } = new List<GameStatistic>();

    public virtual Measurement? IdMeasurementNavigation { get; set; }
}
