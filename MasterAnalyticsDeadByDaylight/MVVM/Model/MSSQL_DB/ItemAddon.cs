using System;
using System.Collections.Generic;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

public partial class ItemAddon
{
    public int IdItemAddon { get; set; }

    public int IdItem { get; set; }

    public string ItemAddonName { get; set; }

    public byte[] ItemAddonImage { get; set; }

    public string ItemAddonDescription { get; set; }

    public virtual ICollection<SurvivorInfo> SurvivorInfoIdAddon1Navigations { get; set; } = new List<SurvivorInfo>();

    public virtual ICollection<SurvivorInfo> SurvivorInfoIdAddon2Navigations { get; set; } = new List<SurvivorInfo>();
}
