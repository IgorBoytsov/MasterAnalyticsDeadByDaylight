using System;
using System.Collections.Generic;

namespace DBDAnalytics.Infrastructure.Models;

public partial class OfferingCategory
{
    public int IdCategory { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Offering> Offerings { get; set; } = new List<Offering>();
}
