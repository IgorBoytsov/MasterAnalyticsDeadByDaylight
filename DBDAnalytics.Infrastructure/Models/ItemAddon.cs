using System;
using System.Collections.Generic;

namespace DBDAnalytics.Infrastructure.Models;

public partial class ItemAddon
{
    public int IdItemAddon { get; set; }

    public int IdItem { get; set; }

    public int? IdRarity { get; set; }

    public string ItemAddonName { get; set; } = null!;

    public byte[]? ItemAddonImage { get; set; }

    public string? ItemAddonDescription { get; set; }

    public virtual Item IdItemNavigation { get; set; } = null!;

    public virtual Rarity? IdRarityNavigation { get; set; }

    public virtual ICollection<SurvivorBuild> SurvivorBuildIdAddonItem1Navigations { get; set; } = new List<SurvivorBuild>();

    public virtual ICollection<SurvivorBuild> SurvivorBuildIdAddonItem2Navigations { get; set; } = new List<SurvivorBuild>();

    public virtual ICollection<SurvivorInfo> SurvivorInfoIdAddon1Navigations { get; set; } = new List<SurvivorInfo>();

    public virtual ICollection<SurvivorInfo> SurvivorInfoIdAddon2Navigations { get; set; } = new List<SurvivorInfo>();
}
