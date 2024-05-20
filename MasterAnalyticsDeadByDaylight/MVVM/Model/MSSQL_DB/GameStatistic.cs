using System;
using System.Collections.Generic;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

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

    public DateTime DateTimeMatch { get; set; }

    public DateTime GameTimeMatch { get; set; }

    public int CountKills { get; set; }

    public int CountHooks { get; set; }

    public int NumberRecentGenerators { get; set; }

    public string DescriptionGame { get; set; }

    public virtual GameEvent IdGameEventNavigation { get; set; }

    public virtual GameMode IdGameModeNavigation { get; set; }

    public virtual KillerInfo IdKillerNavigation { get; set; }

    public virtual Map IdMapNavigation { get; set; }

    public virtual Patch IdPatchNavigation { get; set; }

    public virtual SurvivorInfo IdSurvivors1Navigation { get; set; }

    public virtual SurvivorInfo IdSurvivors2Navigation { get; set; }

    public virtual SurvivorInfo IdSurvivors3Navigation { get; set; }

    public virtual SurvivorInfo IdSurvivors4Navigation { get; set; }

    public virtual WhoPlacedMap IdWhoPlacedMapNavigation { get; set; }

    public virtual WhoPlacedMap IdWhoPlacedMapWinNavigation { get; set; }
}
