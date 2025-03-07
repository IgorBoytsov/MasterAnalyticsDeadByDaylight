using System;
using System.Collections.Generic;

namespace DBDAnalytics.Infrastructure.Models;

public partial class TypeDeath
{
    public int IdTypeDeath { get; set; }

    public string TypeDeathName { get; set; } = null!;

    public string? TypeDeathDescription { get; set; }

    public virtual ICollection<SurvivorInfo> SurvivorInfos { get; set; } = new List<SurvivorInfo>();
}
