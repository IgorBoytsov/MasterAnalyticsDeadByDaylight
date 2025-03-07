using System;
using System.Collections.Generic;

namespace DBDAnalytics.Infrastructure.Models;

public partial class Killer
{
    public int IdKiller { get; set; }

    public string KillerName { get; set; } = null!;

    public byte[]? KillerImage { get; set; }

    public byte[]? KillerAbilityImage { get; set; }

    public virtual ICollection<KillerAddon> KillerAddons { get; set; } = new List<KillerAddon>();

    public virtual ICollection<KillerBuild> KillerBuilds { get; set; } = new List<KillerBuild>();

    public virtual ICollection<KillerInfo> KillerInfos { get; set; } = new List<KillerInfo>();

    public virtual ICollection<KillerPerk> KillerPerks { get; set; } = new List<KillerPerk>();
}
