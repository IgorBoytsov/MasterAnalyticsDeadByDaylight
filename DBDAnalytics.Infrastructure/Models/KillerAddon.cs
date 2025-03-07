﻿using System;
using System.Collections.Generic;

namespace DBDAnalytics.Infrastructure.Models;

public partial class KillerAddon
{
    public int IdKillerAddon { get; set; }

    public int IdKiller { get; set; }

    public int? IdRarity { get; set; }

    public string AddonName { get; set; } = null!;

    public byte[]? AddonImage { get; set; }

    public string? AddonDescription { get; set; }

    public virtual Killer IdKillerNavigation { get; set; } = null!;

    public virtual Rarity? IdRarityNavigation { get; set; }

    public virtual ICollection<KillerBuild> KillerBuildIdAddon1Navigations { get; set; } = new List<KillerBuild>();

    public virtual ICollection<KillerBuild> KillerBuildIdAddon2Navigations { get; set; } = new List<KillerBuild>();

    public virtual ICollection<KillerInfo> KillerInfoIdAddon1Navigations { get; set; } = new List<KillerInfo>();

    public virtual ICollection<KillerInfo> KillerInfoIdAddon2Navigations { get; set; } = new List<KillerInfo>();
}
