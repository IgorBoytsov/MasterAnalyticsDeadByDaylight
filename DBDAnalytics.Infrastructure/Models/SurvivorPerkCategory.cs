using System;
using System.Collections.Generic;

namespace DBDAnalytics.Infrastructure.Models;

public partial class SurvivorPerkCategory
{
    public int IdSurvivorPerkCategory { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<SurvivorPerk> SurvivorPerks { get; set; } = new List<SurvivorPerk>();
}
