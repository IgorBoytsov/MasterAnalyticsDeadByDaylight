using System;
using System.Collections.Generic;

namespace DBDAnalytics.Infrastructure.Models;

public partial class GameStatistic
{
    public int IdGameStatistic { get; set; }

    public int IdKiller { get; set; }

    public int IdMap { get; set; }

    public int IdWhoPlacedMap { get; set; }

    public int IdWhoPlacedMapWin { get; set; }

    public int IdPatch { get; set; }

    public int IdGameMode { get; set; }

    public int IdGameEvent { get; set; }

    public int IdSurvivors1 { get; set; }

    public int IdSurvivors2 { get; set; }

    public int IdSurvivors3 { get; set; }

    public int IdSurvivors4 { get; set; }

    public DateTime? DateTimeMatch { get; set; }

    public string? GameTimeMatch { get; set; }

    public int CountKills { get; set; }

    public int CountHooks { get; set; }

    public int NumberRecentGenerators { get; set; }

    public string? DescriptionGame { get; set; }

    public byte[]? ResultMatch { get; set; }

    public int? IdMatchAttribute { get; set; }

    public virtual GameEvent IdGameEventNavigation { get; set; } = null!;

    public virtual GameMode IdGameModeNavigation { get; set; } = null!;

    public virtual KillerInfo IdKillerNavigation { get; set; } = null!;

    public virtual Map IdMapNavigation { get; set; } = null!;

    public virtual MatchAttribute? IdMatchAttributeNavigation { get; set; }

    public virtual Patch IdPatchNavigation { get; set; } = null!;

    public virtual SurvivorInfo IdSurvivors1Navigation { get; set; } = null!;

    public virtual SurvivorInfo IdSurvivors2Navigation { get; set; } = null!;

    public virtual SurvivorInfo IdSurvivors3Navigation { get; set; } = null!;

    public virtual SurvivorInfo IdSurvivors4Navigation { get; set; } = null!;

    public virtual WhoPlacedMap IdWhoPlacedMapNavigation { get; set; } = null!;

    public virtual WhoPlacedMap IdWhoPlacedMapWinNavigation { get; set; } = null!;
}
