using System;
using System.Collections.Generic;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

public partial class Killer
{
    public int IdKiller { get; set; }

    public string KillerName { get; set; }

    public byte[] KillerImage { get; set; }

    public byte[] KillerAbilityImage { get; set; }

    public virtual ICollection<KillerAddon> KillerAddons { get; set; } = new List<KillerAddon>();

    public virtual ICollection<KillerInfo> KillerInfos { get; set; } = new List<KillerInfo>();

    public virtual ICollection<KillerPerk> KillerPerks { get; set; } = new List<KillerPerk>();
}
