using System;
using System.Collections.Generic;

namespace DBDAnalytics.Infrastructure.Models;

public partial class KillerPerkCategory
{
    public int IdKillerPerkCategory { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<KillerPerk> KillerPerks { get; set; } = new List<KillerPerk>();
}
