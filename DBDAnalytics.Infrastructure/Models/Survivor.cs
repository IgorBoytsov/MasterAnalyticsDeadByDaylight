using System;
using System.Collections.Generic;

namespace DBDAnalytics.Infrastructure.Models;

public partial class Survivor
{
    public int IdSurvivor { get; set; }

    public string SurvivorName { get; set; } = null!;

    public byte[]? SurvivorImage { get; set; }

    public string? SurvivorDescription { get; set; }

    public virtual ICollection<SurvivorInfo> SurvivorInfos { get; set; } = new List<SurvivorInfo>();

    public virtual ICollection<SurvivorPerk> SurvivorPerks { get; set; } = new List<SurvivorPerk>();
}
