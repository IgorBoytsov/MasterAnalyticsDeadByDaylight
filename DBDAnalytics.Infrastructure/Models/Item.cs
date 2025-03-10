﻿using System;
using System.Collections.Generic;

namespace DBDAnalytics.Infrastructure.Models;

public partial class Item
{
    public int IdItem { get; set; }

    public string ItemName { get; set; } = null!;

    public byte[]? ItemImage { get; set; }

    public string? ItemDescription { get; set; }

    public virtual ICollection<ItemAddon> ItemAddons { get; set; } = new List<ItemAddon>();

    public virtual ICollection<SurvivorBuild> SurvivorBuilds { get; set; } = new List<SurvivorBuild>();

    public virtual ICollection<SurvivorInfo> SurvivorInfos { get; set; } = new List<SurvivorInfo>();
}
