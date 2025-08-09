using System;
using System.Collections.Generic;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

public partial class Rarity
{
    public int IdRarity { get; set; }

    public string RarityName { get; set; }

    public virtual ICollection<ItemAddon> ItemAddons { get; set; } = new List<ItemAddon>();

    public virtual ICollection<KillerAddon> KillerAddons { get; set; } = new List<KillerAddon>();

    public virtual ICollection<Offering> Offerings { get; set; } = new List<Offering>();
}
