using System;
using System.Collections.Generic;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

public partial class Offering
{
    public int IdOffering { get; set; }

    public int IdRole { get; set; }

    public int? IdCategory { get; set; }

    public int? IdRarity { get; set; }

    public string OfferingName { get; set; }

    public byte[] OfferingImage { get; set; }

    public string OfferingDescription { get; set; }

    public virtual OfferingCategory IdCategoryNavigation { get; set; }

    public virtual Rarity IdRarityNavigation { get; set; }

    public virtual Role IdRoleNavigation { get; set; }

    public virtual ICollection<KillerInfo> KillerInfos { get; set; } = new List<KillerInfo>();

    public virtual ICollection<SurvivorInfo> SurvivorInfos { get; set; } = new List<SurvivorInfo>();
}
