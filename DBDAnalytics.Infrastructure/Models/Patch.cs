using System;
using System.Collections.Generic;

namespace DBDAnalytics.Infrastructure.Models;

public partial class Patch
{
    public int IdPatch { get; set; }

    public string PatchNumber { get; set; } = null!;

    public DateOnly PatchDateRelease { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<GameStatistic> GameStatistics { get; set; } = new List<GameStatistic>();
}
