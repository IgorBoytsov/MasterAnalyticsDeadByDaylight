using System;
using System.Collections.Generic;

namespace DBDAnalytics.Infrastructure.Models;

public partial class KillerInfo
{
    public int IdKillerInfo { get; set; }

    public int IdKiller { get; set; }

    public int? IdPerk1 { get; set; }

    public int? IdPerk2 { get; set; }

    public int? IdPerk3 { get; set; }

    public int? IdPerk4 { get; set; }

    public int? IdAddon1 { get; set; }

    public int? IdAddon2 { get; set; }

    public int IdAssociation { get; set; }

    public int IdPlatform { get; set; }

    public int? IdKillerOffering { get; set; }

    public int Prestige { get; set; }

    public bool Bot { get; set; }

    public bool AnonymousMode { get; set; }

    public int KillerAccount { get; set; }

    public virtual ICollection<GameStatistic> GameStatistics { get; set; } = new List<GameStatistic>();

    public virtual KillerAddon? IdAddon1Navigation { get; set; }

    public virtual KillerAddon? IdAddon2Navigation { get; set; }

    public virtual PlayerAssociation IdAssociationNavigation { get; set; } = null!;

    public virtual Killer IdKillerNavigation { get; set; } = null!;

    public virtual Offering? IdKillerOfferingNavigation { get; set; }

    public virtual KillerPerk? IdPerk1Navigation { get; set; }

    public virtual KillerPerk? IdPerk2Navigation { get; set; }

    public virtual KillerPerk? IdPerk3Navigation { get; set; }

    public virtual KillerPerk? IdPerk4Navigation { get; set; }

    public virtual Platform IdPlatformNavigation { get; set; } = null!;
}
