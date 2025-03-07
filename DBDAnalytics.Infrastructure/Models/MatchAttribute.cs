using System;
using System.Collections.Generic;

namespace DBDAnalytics.Infrastructure.Models;

public partial class MatchAttribute
{
    public int IdMatchAttribute { get; set; }

    public string AttributeName { get; set; } = null!;

    public string? AttributeDescription { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsHide { get; set; }

    public virtual ICollection<GameStatistic> GameStatistics { get; set; } = new List<GameStatistic>();
}
