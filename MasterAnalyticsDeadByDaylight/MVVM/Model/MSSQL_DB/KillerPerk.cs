using System;
using System.Collections.Generic;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

public partial class KillerPerk
{
    public int IdKillerPerk { get; set; }

    public int IdKiller { get; set; }

    public string PerkName { get; set; }

    public byte[] PerkImage { get; set; }

    public string PerkDescription { get; set; }

    public virtual Killer IdKillerNavigation { get; set; }

    public virtual ICollection<KillerInfo> KillerInfoIdPerk1Navigations { get; set; } = new List<KillerInfo>();

    public virtual ICollection<KillerInfo> KillerInfoIdPerk2Navigations { get; set; } = new List<KillerInfo>();

    public virtual ICollection<KillerInfo> KillerInfoIdPerk3Navigations { get; set; } = new List<KillerInfo>();

    public virtual ICollection<KillerInfo> KillerInfoIdPerk4Navigations { get; set; } = new List<KillerInfo>();
}
