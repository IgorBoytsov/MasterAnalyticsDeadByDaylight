using System;
using System.Collections.Generic;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

public partial class PlayerAssociation
{
    public int IdPlayerAssociation { get; set; }

    public string PlayerAssociationName { get; set; }

    public string PlayerAssociationDescription { get; set; }

    public virtual ICollection<KillerInfo> KillerInfos { get; set; } = new List<KillerInfo>();

    public virtual ICollection<SurvivorInfo> SurvivorInfos { get; set; } = new List<SurvivorInfo>();
}
