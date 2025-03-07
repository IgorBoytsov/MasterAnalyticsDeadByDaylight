using System;
using System.Collections.Generic;

namespace DBDAnalytics.Infrastructure.Models;

public partial class SurvivorBuild
{
    public int IdBuild { get; set; }

    public string? Name { get; set; }

    public int IdPerk1 { get; set; }

    public int IdPerk2 { get; set; }

    public int IdPerk3 { get; set; }

    public int IdPerk4 { get; set; }

    public int IdItem { get; set; }

    public int IdAddonItem1 { get; set; }

    public int IdAddonItem2 { get; set; }

    public virtual ItemAddon IdAddonItem1Navigation { get; set; } = null!;

    public virtual ItemAddon IdAddonItem2Navigation { get; set; } = null!;

    public virtual Item IdItemNavigation { get; set; } = null!;

    public virtual SurvivorPerk IdPerk1Navigation { get; set; } = null!;

    public virtual SurvivorPerk IdPerk2Navigation { get; set; } = null!;

    public virtual SurvivorPerk IdPerk3Navigation { get; set; } = null!;

    public virtual SurvivorPerk IdPerk4Navigation { get; set; } = null!;
}
