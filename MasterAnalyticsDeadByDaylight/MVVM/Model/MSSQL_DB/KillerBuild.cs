using System;
using System.Collections.Generic;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

public partial class KillerBuild
{
    public int IdBuild { get; set; }

    public int IdKiller { get; set; }

    public string Name { get; set; }

    public int IdPerk1 { get; set; }

    public int IdPerk2 { get; set; }

    public int IdPerk3 { get; set; }

    public int IdPerk4 { get; set; }

    public int IdAddon1 { get; set; }

    public int IdAddon2 { get; set; }

    public virtual KillerAddon IdAddon1Navigation { get; set; }

    public virtual KillerAddon IdAddon2Navigation { get; set; }

    public virtual Killer IdKillerNavigation { get; set; }

    public virtual KillerPerk IdPerk1Navigation { get; set; }

    public virtual KillerPerk IdPerk2Navigation { get; set; }

    public virtual KillerPerk IdPerk3Navigation { get; set; }

    public virtual KillerPerk IdPerk4Navigation { get; set; }
}
