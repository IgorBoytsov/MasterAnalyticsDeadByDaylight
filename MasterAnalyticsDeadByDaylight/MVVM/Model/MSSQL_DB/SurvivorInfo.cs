using System;
using System.Collections.Generic;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

public partial class SurvivorInfo
{
    public int IdSurvivorInfo { get; set; }

    public int IdSurvivor { get; set; }

    public int? IdPerk1 { get; set; }

    public int? IdPerk2 { get; set; }

    public int? IdPerk3 { get; set; }

    public int? IdPerk4 { get; set; }

    public int? IdItem { get; set; }

    public int? IdAddon1 { get; set; }

    public int? IdAddon2 { get; set; }

    public int IdTypeDeath { get; set; }

    public int IdAssociation { get; set; }

    public int IdPlatform { get; set; }

    public int? IdSurvivorOffering { get; set; }

    public int Prestige { get; set; }

    public bool Bot { get; set; }

    public bool AnonymousMode { get; set; }

    public int SurvivorAccount { get; set; }

    public virtual ICollection<GameStatistic> GameStatisticIdSurvivors1Navigations { get; set; } = new List<GameStatistic>();

    public virtual ICollection<GameStatistic> GameStatisticIdSurvivors2Navigations { get; set; } = new List<GameStatistic>();

    public virtual ICollection<GameStatistic> GameStatisticIdSurvivors3Navigations { get; set; } = new List<GameStatistic>();

    public virtual ICollection<GameStatistic> GameStatisticIdSurvivors4Navigations { get; set; } = new List<GameStatistic>();

    public virtual ItemAddon IdAddon1Navigation { get; set; }

    public virtual ItemAddon IdAddon2Navigation { get; set; }

    public virtual PlayerAssociation IdAssociationNavigation { get; set; }

    public virtual Item IdItemNavigation { get; set; }

    public virtual SurvivorPerk IdPerk1Navigation { get; set; }

    public virtual SurvivorPerk IdPerk2Navigation { get; set; }

    public virtual SurvivorPerk IdPerk3Navigation { get; set; }

    public virtual SurvivorPerk IdPerk4Navigation { get; set; }

    public virtual Platform IdPlatformNavigation { get; set; }

    public virtual Survivor IdSurvivorNavigation { get; set; }

    public virtual Offering IdSurvivorOfferingNavigation { get; set; }

    public virtual TypeDeath IdTypeDeathNavigation { get; set; }
}
