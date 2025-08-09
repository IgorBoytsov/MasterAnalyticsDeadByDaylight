using System;
using System.Collections.Generic;

namespace DBDAnalytics.Infrastructure.Models;

public partial class SurvivorPerk
{
    public int IdSurvivorPerk { get; set; }

    public int IdSurvivor { get; set; }

    public string PerkName { get; set; } = null!;

    public byte[]? PerkImage { get; set; }

    public int? IdCategory { get; set; }

    public string? PerkDescription { get; set; }

    public virtual SurvivorPerkCategory? IdCategoryNavigation { get; set; }

    public virtual Survivor IdSurvivorNavigation { get; set; } = null!;

    public virtual ICollection<SurvivorBuild> SurvivorBuildIdPerk1Navigations { get; set; } = new List<SurvivorBuild>();

    public virtual ICollection<SurvivorBuild> SurvivorBuildIdPerk2Navigations { get; set; } = new List<SurvivorBuild>();

    public virtual ICollection<SurvivorBuild> SurvivorBuildIdPerk3Navigations { get; set; } = new List<SurvivorBuild>();

    public virtual ICollection<SurvivorBuild> SurvivorBuildIdPerk4Navigations { get; set; } = new List<SurvivorBuild>();

    public virtual ICollection<SurvivorInfo> SurvivorInfoIdPerk1Navigations { get; set; } = new List<SurvivorInfo>();

    public virtual ICollection<SurvivorInfo> SurvivorInfoIdPerk2Navigations { get; set; } = new List<SurvivorInfo>();

    public virtual ICollection<SurvivorInfo> SurvivorInfoIdPerk3Navigations { get; set; } = new List<SurvivorInfo>();

    public virtual ICollection<SurvivorInfo> SurvivorInfoIdPerk4Navigations { get; set; } = new List<SurvivorInfo>();
}
