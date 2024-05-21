using System;
using System.Collections.Generic;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

public partial class SurvivorPerk
{
    public int IdSurvivorPerk { get; set; }

    public int IdSurvivor { get; set; }

    public string PerkName { get; set; }

    public byte[] PerkImage { get; set; }

    public string PerkDescription { get; set; }

    public virtual Survivor IdSurvivorNavigation { get; set; }

    public virtual ICollection<SurvivorInfo> SurvivorInfoIdPerk1Navigations { get; set; } = new List<SurvivorInfo>();

    public virtual ICollection<SurvivorInfo> SurvivorInfoIdPerk2Navigations { get; set; } = new List<SurvivorInfo>();

    public virtual ICollection<SurvivorInfo> SurvivorInfoIdPerk3Navigations { get; set; } = new List<SurvivorInfo>();

    public virtual ICollection<SurvivorInfo> SurvivorInfoIdPerk4Navigations { get; set; } = new List<SurvivorInfo>();
}
